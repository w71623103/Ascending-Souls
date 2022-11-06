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

