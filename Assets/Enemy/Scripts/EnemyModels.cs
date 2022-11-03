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
    public int currentDestination;
    public void nextPos()
    {
        currentDestination++;
        if(currentDestination >= patrolDestination.Length) currentDestination = 0;
    }
    public float patrolSpeed;
}