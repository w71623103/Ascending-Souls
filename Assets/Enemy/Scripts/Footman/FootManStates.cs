using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region FootMan States
public abstract class FootManStateBase
{
    public abstract void EnterState(FootMan em);
    public abstract void Update(FootMan em);
    public abstract void FixedUpdate(FootMan em);
    public abstract void ExitState(FootMan em);
}

public class FootManIdleState : FootManStateBase
{
    public override void EnterState(FootMan em)
    {
        em.statevisualizer = Enemy.enemyStates.Idle;
        //Reset Idle Timer;
        em.idleModel.idleTimer = em.idleModel.idleTimerMax;
        em.enemyAnim.Play("Idle");
        /*em.enemyRB.velocity = Vector2.zero;*/
    }
    public override void Update(FootMan em)
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
            if (Physics2D.OverlapCircle(em.attackModel.attackPointNormal.position, em.attackModel.attackNormalRange, em.playerMask))
            {//Player in attack range
                em.ChangeState(em.attackState);
            }
            else if (em.idleModel.idleTimer <= 0f) //Idle Timer end
            {
                em.ChangeState(em.patrolState);
            }
        }
        else if (em.idleModel.idleTimer <= 0f) //Idle Timer end
        {
            em.ChangeState(em.patrolState);
        }
        //Switch State Logic=============
    }
    public override void FixedUpdate(FootMan em)
    {
        
    }
    public override void ExitState(FootMan em)
    {
        em.idleModel.idleTimer = 0f;
    }
}

public class FootManHurtState : FootManStateBase
{
    public override void EnterState(FootMan em)
    {
        em.statevisualizer = Enemy.enemyStates.Hurt;
        //em.enemyAnim.SetTrigger("isHurt");
        em.enemyAnim.Play("Hurt");
        em.attackModel.attackSPHate += 0.5f;
    }
    public override void Update(FootMan em)
    {
        bool isFinished = em.enemyAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= .95f;

        if (isFinished)
        {
            em.ChangeState(em.idleState);
        }
    }
    public override void FixedUpdate(FootMan em)
    {

    }
    public override void ExitState(FootMan em)
    {
        if ((int)em.moveModel.Direction == em.hitDir.x)
            em.turn();

        em.hitDir = Vector2.zero;
        //em.enemyRB.gravityScale = 3f;

        if (em.moveModel.isGrounded)
            em.enemyRB.gravityScale = 3f;
        else
            em.enemyRB.gravityScale = 2f;
        //em.enemyRB.velocity = Vector2.zero;
    }
}

public class FootManPatrolState : FootManStateBase
{
    public override void EnterState(FootMan em)
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
    public override void Update(FootMan em)
    {
        //Switch State Logic
        if (em.target != null && !(em.transform.position.x < em.chaseModel.chaseRange[0].position.x || em.transform.position.x > em.chaseModel.chaseRange[1].position.x)) //Player in Sight
        {
            em.ChangeState(em.chaseState);
        }
        else if (em.target != null && (em.transform.position.x < em.chaseModel.chaseRange[0].position.x || em.transform.position.x > em.chaseModel.chaseRange[1].position.x)) //Player in Sight
        {
            if (Physics2D.OverlapCircle(em.attackModel.attackPointNormal.position, em.attackModel.attackNormalRange, em.playerMask))
            {//Player in attack range
                em.ChangeState(em.attackState);
            }
        }
        else if (em.patrolModel.reached) //Reach patrol pos
        {
            em.patrolModel.nextPos();
            em.ChangeState(em.idleState);
        }
    }
    public override void FixedUpdate(FootMan em)
    {
        //walk to next pos
        if (Mathf.Abs(em.transform.position.x - em.patrolModel.patrolDestination[em.patrolModel.currentDestination].position.x) > em.patrolModel.patrolTolerance)
            em.enemyRB.velocity = new Vector2((int)em.moveModel.Direction, em.enemyRB.velocity.y) * em.patrolModel.patrolSpeed;
        else
            em.patrolModel.reached = true;
    }
    public override void ExitState(FootMan em)
    {
        em.enemyRB.velocity = Vector2.zero;
    }
}

public class FootManChaseState : FootManStateBase
{
    private bool isMoving;
    public override void EnterState(FootMan em)
    {
        em.statevisualizer = Enemy.enemyStates.Chase;
        em.enemyAnim.Play("Run");
        isMoving = true;
    }
    public override void Update(FootMan em)
    {
        em.attackModel.attackSPHate += Time.deltaTime;
        if (em.target == null || (em.transform.position.x < em.chaseModel.chaseRange[0].position.x || em.transform.position.x > em.chaseModel.chaseRange[1].position.x))
        {
            em.ChangeState(em.idleState);
        }
        else
        {
            if (Physics2D.OverlapCircle(em.attackModel.attackPointNormal.position, em.attackModel.attackNormalRange, em.playerMask))
            {//Player in attack range
                em.ChangeState(em.attackState);
            }
            else
            {
                isMoving = true;
            }
        }
        
    }
    public override void FixedUpdate(FootMan em)
    {
        if(isMoving) em.enemyRB.velocity = new Vector2((int)em.moveModel.Direction, 0) * em.chaseModel.chaseSpeed; 
    }
    public override void ExitState(FootMan em)
    {
        em.enemyRB.velocity = Vector2.zero;
    }
}

public class FootManAttackState : FootManStateBase
{
    public override void EnterState(FootMan em)
    {
        em.statevisualizer = Enemy.enemyStates.Attack;
        em.enemyAnim.Play("AttackN");
    }
    public override void Update(FootMan em)
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
    public override void FixedUpdate(FootMan em)
    {

    }
    public override void ExitState(FootMan em)
    {

    }
}

public class FootManAttackFinishState : FootManStateBase
{
    private float timer = 0f;
    private float timeDiff;
    /*private bool isApproach = true;*/
    public override void EnterState(FootMan em)
    {
        em.statevisualizer = Enemy.enemyStates.AttackFinish;
        em.enemyAnim.Play("Idle");
        timer = 0f;
        timeDiff = Random.Range(-0.5f, 0.5f);
    }
    public override void Update(FootMan em)
    {
        timer += Time.deltaTime;
        if(timer > em.attackModel.attackFinishTime + timeDiff)
        {
            em.ChangeState(em.idleState);
        }
        if (em.attackModel.playerInRange && em.target == null) em.turn();
    }
    public override void FixedUpdate(FootMan em)
    {

    }
    public override void ExitState(FootMan em)
    {
        timer = 0f;
    }
}

public class FootManForocedMoveState : FootManStateBase
{
    public override void EnterState(FootMan em)
    {
        em.statevisualizer = Enemy.enemyStates.Dragged;
        em.enemyAnim.Play("Hurt");
        em.dragModel.timer = 0f;
        em.enemyRB.gravityScale = 0f;
        em.dragModel.emLine.enabled = true;
        em.dragModel.emLine.SetPosition(0, em.transform.position);
        em.dragModel.emLine.SetPosition(1, em.dragModel.targetPosition);
    }
    public override void Update(FootMan em)
    {
        em.dragModel.emLine.SetPosition(0, em.transform.position);
        if (Vector3.Distance(em.transform.position, em.dragModel.targetPosition) > em.dragModel.disTolerance)
        {
            em.transform.position = Vector3.Lerp(em.transform.position, em.dragModel.targetPosition, em.dragModel.timer / em.dragModel.travelTime);
        }
        else
        {
            em.ChangeState(em.idleState);
        }
        em.dragModel.timer += Time.deltaTime;
    }
    public override void FixedUpdate(FootMan em)
    {

    }
    public override void ExitState(FootMan em)
    {
        em.enemyRB.gravityScale = 3f;
        em.dragModel.emLine.enabled = false;
    }
}

#endregion