using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState
{
    public abstract void EnterState(Enemy em);
    public abstract void Update(Enemy em);
    public abstract void FixedUpdate(Enemy em);
    public abstract void ExitState(Enemy em);
}
#region Enemy State
public class EnemyIdleState : EnemyState
{
    public override void EnterState(Enemy em)
    {
        em.statevisualizer = Enemy.enemyStates.Idle;
    }
    public override void Update(Enemy em)
    {

    }
    public override void FixedUpdate(Enemy em)
    {

    }
    public override void ExitState(Enemy em)
    {

    }
}

public class EnemyPatrolState : EnemyState
{
    public override void EnterState(Enemy em)
    {
        em.statevisualizer = Enemy.enemyStates.Patrol;
    }
    public override void Update(Enemy em)
    {

    }
    public override void FixedUpdate(Enemy em)
    {

    }
    public override void ExitState(Enemy em)
    {

    }
}

public class EnemyChaseState : EnemyState
{
    public override void EnterState(Enemy em)
    {
        em.statevisualizer = Enemy.enemyStates.Chase;
    }
    public override void Update(Enemy em)
    {

    }
    public override void FixedUpdate(Enemy em)
    {

    }
    public override void ExitState(Enemy em)
    {

    }
}

public class EnemyAttackState : EnemyState
{
    public override void EnterState(Enemy em)
    {
        em.statevisualizer = Enemy.enemyStates.Attack;
    }
    public override void Update(Enemy em)
    {

    }
    public override void FixedUpdate(Enemy em)
    {

    }
    public override void ExitState(Enemy em)
    {

    }
}

public class EnemyHurtState : EnemyState
{
    public override void EnterState(Enemy em)
    {
        em.statevisualizer = Enemy.enemyStates.Hurt;
    }
    public override void Update(Enemy em)
    {

    }
    public override void FixedUpdate(Enemy em)
    {

    }
    public override void ExitState(Enemy em)
    {

    }
}
#endregion

#region SwordMan States
public class SwordManIdleState : EnemyIdleState
{
    public override void EnterState(Enemy em)
    {
        em.statevisualizer = Enemy.enemyStates.Idle;
        //Reset Idle Timer;
        em.idleModel.idleTimer = em.idleModel.idleTimerMax;
        em.enemyAnim.Play("Idle");
    }
    public override void Update(Enemy em)
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
        if (em.target != null) //Player in Sight
        {
            em.ChangeState(em.chaseState);
        }
        else if(em.idleModel.idleTimer <= 0f) //Idle Timer end
        {
            em.ChangeState(em.patrolState);
        }
        //Switch State Logic=============
    }
    public override void FixedUpdate(Enemy em)
    {
        em.enemyRB.velocity = Vector2.zero;
    }
    public override void ExitState(Enemy em)
    {
        em.idleModel.idleTimer = 0f;
    }
}

public class SwordManHurtState : EnemyHurtState
{
    public override void EnterState(Enemy em)
    {
        em.statevisualizer = Enemy.enemyStates.Hurt;
        //em.enemyAnim.SetTrigger("isHurt");
        em.enemyAnim.Play("Hurt");
    }
    public override void Update(Enemy em)
    {

    }
    public override void FixedUpdate(Enemy em)
    {

    }
    public override void ExitState(Enemy em)
    {
        if ((int)em.moveModel.Direction == em.hitDir.x)
            em.turn();

        em.hitDir = Vector2.zero;
        //em.enemyRB.gravityScale = 3f;
    }
}

public class SwordManPatrolState : EnemyPatrolState
{
    public override void EnterState(Enemy em)
    {
        em.statevisualizer = Enemy.enemyStates.Patrol;
        //turn to next patrol pos;
        if(em.patrolModel.patrolDestination[em.patrolModel.currentDestination].position.x > em.transform.position.x)
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
    public override void Update(Enemy em)
    {
        //walk to next pos
        if (Mathf.Abs(em.transform.position.x - em.patrolModel.patrolDestination[em.patrolModel.currentDestination].position.x) > em.patrolModel.patrolTolerance)
            em.enemyRB.velocity = new Vector2((int)em.moveModel.Direction, em.enemyRB.velocity.y) * em.patrolModel.patrolSpeed;
        else
            em.patrolModel.reached = true;
        //Switch State Logic
        if (em.target != null) //Player in Sight
        {
            em.ChangeState(em.chaseState);
        }
        else if (em.patrolModel.reached) //Reach patrol pos
        {
            em.patrolModel.nextPos();
            em.ChangeState(em.idleState);
        }
    }
    public override void FixedUpdate(Enemy em)
    {

    }
    public override void ExitState(Enemy em)
    {

    }
}

public class SwordManChaseState : EnemyChaseState
{
    public override void EnterState(Enemy em)
    {
        em.statevisualizer = Enemy.enemyStates.Chase;
    }
    public override void Update(Enemy em)
    {

    }
    public override void FixedUpdate(Enemy em)
    {

    }
    public override void ExitState(Enemy em)
    {

    }
}

public class SwordManAttackState : EnemyAttackState
{
    public override void EnterState(Enemy em)
    {
        em.statevisualizer = Enemy.enemyStates.Attack;
    }
    public override void Update(Enemy em)
    {

    }
    public override void FixedUpdate(Enemy em)
    {

    }
    public override void ExitState(Enemy em)
    {

    }
}
#endregion