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
