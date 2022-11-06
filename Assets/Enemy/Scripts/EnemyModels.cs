using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyMoveModel
{
    public enum EnemyDirection
    {
        Left = -1,
        Right = 1,
    }

    public EnemyDirection Direction = EnemyDirection.Right;
    public bool isGrounded;
}

[System.Serializable]
public class EnemyIdleModel
{
    public float idleTimer;
    public float idleTimerMax;
}

[System.Serializable]
public class EnemyPatrolModel
{
    public bool reached;
    public Transform[] patrolDestination;
    public int currentDestination = 0;
    public void nextPos()
    {
        currentDestination++;
        if(currentDestination >= patrolDestination.Length) currentDestination = 0;
    }
    public float patrolSpeed;
    public float patrolTolerance = 0.5f;
}

[System.Serializable]
public class EnemyChaseModel
{
    public float chaseSpeed;
    public Transform[] chaseRange;
}

[System.Serializable]
public class EnemyAttackModel
{
    public Transform attackPointNormal;
    public Transform attackPointSP;
    public float attackNormalRange;
    public float attackSPRange;
    public float attackSPHate = 0f;
    public float attackSPHateGate;

}