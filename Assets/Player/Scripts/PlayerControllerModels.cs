using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerMoveModel
{
    public float HorizontalMovement;
    public float VerticalMovement;
    public float hspeed;

    public enum PlayerDirection
    {
        Left = -1, 
        Right = 1,
    }
    public PlayerDirection Direction;
    public GameObject groundSensor;
}

[System.Serializable]
public class PlayerJumpModel
{
    public float jumpSpeed;
    public float smallJumpSpeed;
    public int jumpCount;
    public int jumpCountMax;
    public bool isGrounded;
    public bool isPlatform;
    public float platformTimer;
    public float platformMaxTimer = 1f;
}

[System.Serializable]
public class PlayerDashModel
{
    public float dashSpeed;
    public bool allowDash = true;
    public float dashCDTimer;
    public float dashCD = 2f;
}

[System.Serializable]
public class PlayerSlideModel
{
    public GameObject slideDetectPos;
    public float slideJumpHorizontalSpeed;
    public float slidingCancelTimer;
    public float slidingCancelTimerMax;
    public bool canSlide;
    public float normalGravity = 3f;
    public float slideGravity = 0.1f;
    public bool wallJumped = false;
}

[System.Serializable]
public class PlayerAttackModel
{
    //This Model only controls variables for the action to work.
    //This is NOT the Model for attack Stats.
    public int attackCount = 0;
    public int attackCountMax = 5;
    public int attackCountAir = 0;
    public int attackCountAirMax = 3;
    public bool allowInput;
    public bool comboed;
    public float attackTimer;
    public float attackTimerMax;
    public float attackTimerAir;
    public float attackTimerAirMax;
    public int airAttackComboCount;
    public int airAttackComboCountMax;
    public bool allowTurn = true;
    public float comboDropTime = 1f;
    public float comboDropTimer = 0f;
    public bool dropComboBool = false;
    public int historyAttacCount;
}

[System.Serializable]
public class PlayerGrappleModel
{
    [Header("MoveGrapple")]
    public GameObject point;
    public float reachTolerance;
    public float directionChangeTolerance;
    public float grappleJumpSpeed;
    public float grappleJumpEndSpeed;
    public float grapplePointExcludeCD;
    public GrappleArea GrappleSensor;
    public LineRenderer playerLine;

    public float weaponGrappleLength;
    public Transform WeaponGrappleStartPos;
    public Transform WeaponGrappleTargetPos;

    [Header("EnemyGrapple")]
    public GameObject enemyPoint;
    public GrappleEnemyArea EnemySensor;
    public Transform enemyGrapplePos;
    public float enemyGrappleTimer;
    public float enemyGrappleTimerMax;
}

[System.Serializable]
public class PlayerWeaponModel
{
    public Weapons currentWeapon;
    public Weapons nextWeapon = null;
    public GameObject WeaponIcon;

    [Header("Sample Weapon Types")]
    public Weapons bareHand;
    public Weapons sword;
    public Weapons greatSword;
    public Weapons dualSword;

}

[System.Serializable]
public class PlayerInteractModel
{
    public Vector2 interactBoxSize;
}

[System.Serializable]
public class PlayerHurtModel
{
    public float flashTime;
}

/*[System.Serializable]
public class PlayerHealModel
{
    public float heal;

    public int healNum;
    public int healNumMax = 5;
}*/