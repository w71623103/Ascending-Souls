using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region SwordMan States
public abstract class SwordManStateBase
{
    public abstract void EnterState(SwordMan em);
    public abstract void Update(SwordMan em);
    public abstract void FixedUpdate(SwordMan em);
    public abstract void ExitState(SwordMan em);
}

public class SwordManIdleState : SwordManStateBase
{
    public override void EnterState(SwordMan em)
    {
        em.statevisualizer = Enemy.enemyStates.Idle;
        //Reset Idle Timer;
        em.idleModel.idleTimer = em.idleModel.idleTimerMax;
        em.enemyAnim.Play("Idle");
        em.enemyRB.velocity = Vector2.zero;
    }
    public override void Update(SwordMan em)
    {
        //Gravity Management
        if (em.moveModel.isGrounded)
            em.enemyRB.gravityScale = 3f;
        else
            em.enemyRB.gravityScale = 2f;
        //Gravity Management=============

        //Reudce Idle Timer
        em.idleModel.idleTimer -= Time.deltaTime;
        //Reudce Idle Timer==============

        //Switch State Logic
        if (em.target != null && !(em.transform.position.x < em.chaseModel.chaseRange[0].position.x || em.transform.position.x > em.chaseModel.chaseRange[1].position.x)) //Player in Sight
        {
            em.ChangeState(em.chaseState);
        }
        else if (em.target != null && (em.transform.position.x < em.chaseModel.chaseRange[0].position.x || em.transform.position.x > em.chaseModel.chaseRange[1].position.x)) //Player in Sight
        {
            if (em.attackModel.attackSPHate < em.attackModel.attackSPHateGate)
            {//Enemy has not reach the time to cast a special attack
                if (Physics2D.OverlapCircle(em.attackModel.attackPointNormal.position, em.attackModel.attackNormalRange, em.playerMask))
                {//Player in attack range
                    em.ChangeState(em.attackState);
                }
                else if (em.idleModel.idleTimer <= 0f) //Idle Timer end
                {
                    em.ChangeState(em.patrolState);
                }
            }
            else
            {//Enemy has reach the time to cast a special attack
                if (Physics2D.OverlapCircle(em.attackModel.attackPointSP.position, em.attackModel.attackSPRange, em.playerMask))
                {//Player in attack range
                    em.ChangeState(em.attackStateSP);
                }
                else if (em.idleModel.idleTimer <= 0f) //Idle Timer end
                {
                    em.ChangeState(em.patrolState);
                }
            }
        }
        else if (em.idleModel.idleTimer <= 0f) //Idle Timer end
        {
            em.ChangeState(em.patrolState);
        }
        //Switch State Logic=============
    }
    public override void FixedUpdate(SwordMan em)
    {
        
    }
    public override void ExitState(SwordMan em)
    {
        em.idleModel.idleTimer = 0f;
    }
}

public class SwordManHurtState : SwordManStateBase
{
    public override void EnterState(SwordMan em)
    {
        em.statevisualizer = Enemy.enemyStates.Hurt;
        //em.enemyAnim.SetTrigger("isHurt");
        em.enemyAnim.Play("Hurt");
        em.attackModel.attackSPHate += 0.5f;
    }
    public override void Update(SwordMan em)
    {
        bool isFinished = em.enemyAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= .95f;

        if (isFinished)
        {
            em.ChangeState(em.idleState);
        }
    }
    public override void FixedUpdate(SwordMan em)
    {

    }
    public override void ExitState(SwordMan em)
    {
        if ((int)em.moveModel.Direction == em.hitDir.x)
            em.turn();

        em.hitDir = Vector2.zero;
        //em.enemyRB.gravityScale = 3f;
    }
}

public class SwordManPatrolState : SwordManStateBase
{
    public override void EnterState(SwordMan em)
    {
        em.statevisualizer = Enemy.enemyStates.Patrol;
        //turn to next patrol pos;
        if (em.patrolModel.patrolDestination[em.patrolModel.currentDestination].position.x > em.transform.position.x)
        {
            em.moveModel.Direction = EnemyMoveModel.EnemyDirection.Right;
        }
        else
        {
            em.moveModel.Direction = EnemyMoveModel.EnemyDirection.Left;
        }
        em.patrolModel.reached = false;
        em.enemyAnim.Play("Run");
    }
    public override void Update(SwordMan em)
    {
        //Switch State Logic
        if (em.target != null && !(em.transform.position.x < em.chaseModel.chaseRange[0].position.x || em.transform.position.x > em.chaseModel.chaseRange[1].position.x)) //Player in Sight
        {
            em.ChangeState(em.chaseState);
        }
        else if (em.target != null && (em.transform.position.x < em.chaseModel.chaseRange[0].position.x || em.transform.position.x > em.chaseModel.chaseRange[1].position.x)) //Player in Sight
        {
            if (em.attackModel.attackSPHate < em.attackModel.attackSPHateGate)
            {//Enemy has not reach the time to cast a special attack
                if (Physics2D.OverlapCircle(em.attackModel.attackPointNormal.position, em.attackModel.attackNormalRange, em.playerMask))
                {//Player in attack range
                    em.ChangeState(em.attackState);
                }
            }
            else
            {//Enemy has reach the time to cast a special attack
                if (Physics2D.OverlapCircle(em.attackModel.attackPointSP.position, em.attackModel.attackSPRange, em.playerMask))
                {//Player in attack range
                    em.ChangeState(em.attackStateSP);
                }
            }
        }
        else if (em.patrolModel.reached) //Reach patrol pos
        {
            em.patrolModel.nextPos();
            em.ChangeState(em.idleState);
        }
    }
    public override void FixedUpdate(SwordMan em)
    {
        //walk to next pos
        if (Mathf.Abs(em.transform.position.x - em.patrolModel.patrolDestination[em.patrolModel.currentDestination].position.x) > em.patrolModel.patrolTolerance)
            em.enemyRB.velocity = new Vector2((int)em.moveModel.Direction, em.enemyRB.velocity.y) * em.patrolModel.patrolSpeed;
        else
            em.patrolModel.reached = true;
    }
    public override void ExitState(SwordMan em)
    {

    }
}

public class SwordManChaseState : SwordManStateBase
{
    private bool isMoving;
    public override void EnterState(SwordMan em)
    {
        em.statevisualizer = Enemy.enemyStates.Chase;
        em.enemyAnim.Play("Run");
        isMoving = true;
    }
    public override void Update(SwordMan em)
    {
        em.attackModel.attackSPHate += Time.deltaTime;
        if (em.target == null || (em.transform.position.x < em.chaseModel.chaseRange[0].position.x || em.transform.position.x > em.chaseModel.chaseRange[1].position.x))
        {
            em.ChangeState(em.idleState);
        }
        else
        {
            if (em.attackModel.attackSPHate < em.attackModel.attackSPHateGate)
            {//Enemy has not reach the time to cast a special attack
                if (Physics2D.OverlapCircle(em.attackModel.attackPointNormal.position, em.attackModel.attackNormalRange, em.playerMask))
                {//Player in attack range
                    em.ChangeState(em.attackState);
                }
                else
                {
                    isMoving = true;
                }
            }
            else
            {//Enemy has reach the time to cast a special attack
                if (Physics2D.OverlapCircle(em.attackModel.attackPointSP.position, em.attackModel.attackSPRange, em.playerMask))
                {//Player in attack range
                    em.ChangeState(em.attackStateSP);
                }
                else
                {
                    isMoving = true;
                }
            }
        }
        
    }
    public override void FixedUpdate(SwordMan em)
    {
        if(isMoving) em.enemyRB.velocity = new Vector2((int)em.moveModel.Direction, 0) * em.chaseModel.chaseSpeed; 
    }
    public override void ExitState(SwordMan em)
    {

    }
}

public class SwordManAttackState : SwordManStateBase
{
    public override void EnterState(SwordMan em)
    {
        em.statevisualizer = Enemy.enemyStates.Attack;
        em.enemyAnim.Play("AttackN");
    }
    public override void Update(SwordMan em)
    {
        em.attackModel.attackSPHate += Time.deltaTime;
        bool isFinished = em.enemyAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= .95f;

        if (isFinished)
        {
            /*if(Physics2D.OverlapCircle(em.attackModel.attackPointSP.position, em.attackModel.attackSPRange, em.playerMask))
            {
                em.ChangeState(em.chaseState);
            }
            else
            {
                em.ChangeState(em.idleState);
            }*/
            em.ChangeState(em.attackFinishState);
        }
    }
    public override void FixedUpdate(SwordMan em)
    {

    }
    public override void ExitState(SwordMan em)
    {

    }
}

public class SwordManAttackSPState : SwordManStateBase
{
    public override void EnterState(SwordMan em)
    {
        //em.allowHitRecover = false;
        em.statevisualizer = Enemy.enemyStates.Attack;
        em.enemyAnim.Play("AttackSP");
        em.attackModel.attackSPHate = 0f;
    }
    public override void Update(SwordMan em)
    {
        bool isFinished = em.enemyAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= .95f;

        if (isFinished)
        {
            /*if (Physics2D.OverlapCircle(em.attackModel.attackPointSP.position, em.attackModel.attackSPRange, em.playerMask))
            {
                em.ChangeState(em.chaseState);
            }
            else
            {
                em.ChangeState(em.idleState);
            }*/
            em.ChangeState(em.attackFinishState);
        }
    }
    public override void FixedUpdate(SwordMan em)
    {

    }
    public override void ExitState(SwordMan em)
    {
        //em.allowHitRecover = true;
        em.attackModel.attackSPHate = 0f;
    }
}

public class SwordManAttackFinishState : SwordManStateBase
{
    private float timer = 0f;
    private float timeDiff;
    private bool isApproach = true;
    public override void EnterState(SwordMan em)
    {
        em.statevisualizer = Enemy.enemyStates.AttackFinish;
        em.enemyAnim.Play("Run");
        timer = 0f;
        timeDiff = Random.Range(-0.5f, 0.5f);
        if(em.target != null && em.attackModel.hatePercent >= 0.5f)
        {
            isApproach = true;
        }
        else
        {
            isApproach = false;
        }
    }
    public override void Update(SwordMan em)
    {
        timer += Time.deltaTime;
        if(timer > em.attackModel.attackFinishTime + timeDiff)
        {
            em.ChangeState(em.idleState);
        }
        if (em.attackModel.playerInRange && em.target == null) em.turn();
    }
    public override void FixedUpdate(SwordMan em)
    {
        if(isApproach) em.enemyRB.velocity = new Vector2((int)em.moveModel.Direction, em.enemyRB.velocity.y) * em.attackModel.attackFinishMoveSpeed;
        else em.enemyRB.velocity = new Vector2(-1 * (int)em.moveModel.Direction, em.enemyRB.velocity.y) * em.attackModel.attackFinishMoveSpeed;
    }
    public override void ExitState(SwordMan em)
    {
        timer = 0f;
    }
}
#endregion