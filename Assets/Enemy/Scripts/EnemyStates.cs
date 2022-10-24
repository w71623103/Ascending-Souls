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

public class SwordManIdleState : EnemyIdleState
{
    public override void EnterState(Enemy em)
    {
        em.statevisualizer = Enemy.enemyStates.Idle;
        //em.enemyRB.gravityScale = 3f;
    }
    public override void Update(Enemy em)
    {
        if (em.moveModel.isGrounded)
            em.enemyRB.gravityScale = 3f;
        else
            em.enemyRB.gravityScale = 2f;
    }
    public override void FixedUpdate(Enemy em)
    {
        em.enemyRB.velocity = Vector2.zero;
    }
    public override void ExitState(Enemy em)
    {

    }
}

public class SwordManHurtState : EnemyHurtState
{
    public override void EnterState(Enemy em)
    {
        em.statevisualizer = Enemy.enemyStates.Hurt;
        em.enemyAnim.SetTrigger("isHurt");
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