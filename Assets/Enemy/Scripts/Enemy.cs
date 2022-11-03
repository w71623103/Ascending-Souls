using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public enum enemyStates
    {
        Idle,
        Hurt,
        Patrol,
        Chase,
        Attack,
    }
    public EnemyMoveModel moveModel = new EnemyMoveModel();
    public EnemyIdleModel idleModel = new EnemyIdleModel();
    public EnemyPatrolModel patrolModel = new EnemyPatrolModel();

    public enemyStates statevisualizer;
    [Header("Component")]
    [SerializeField] public Rigidbody2D enemyRB;
    [SerializeField] public Animator enemyAnim;
    [SerializeField] public SpriteRenderer enemySP;
    [SerializeField] public SoundManager_Enemy soundM;
    [SerializeField] protected Hp myHp;

    [Header("Hit")]
    [SerializeField] protected bool allowHitRecover = true;
    [SerializeField] public Vector2 hitDir;
    [SerializeField] public float flashTime = 0.1f;
    //[SerializeField] protected GameObject blood;

    [Header("Ground check")]
    [SerializeField] protected Transform groundSensorStartPos;
    [SerializeField] protected float groundHitDis = 0.2f;
    protected LayerMask groundMask;

    [Header("Player check")]
    [SerializeField] protected Transform eyePos;
    public GameObject target;
    [SerializeField] protected float sightDis = 5f;
    protected LayerMask playerMask;
    //
    #region FSM
    //The states All Enemies will share
    public EnemyState generalState;
    public EnemyIdleState idleState;
    public EnemyHurtState hurtState;
    public EnemyAttackState attackState;
    public EnemyPatrolState patrolState;
    public EnemyChaseState chaseState;

    public void ChangeState(EnemyState newState)
    {
        if (generalState != null)
        {
            generalState.ExitState(this);
        }
        generalState = newState;
        if (generalState != null)
        {
            generalState.EnterState(this);
        }
    }
    #endregion

    // Start here is just for sample when creating new enemies
    // the following code should appear in every kind of enemies
    // copy and paste
    void Start()
    {
        groundMask = LayerMask.GetMask("Ground");
        enemyRB = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
        enemySP = GetComponent<SpriteRenderer>();

        ChangeState(idleState);
    }

    public abstract void OnHit(Vector2 hitBackDir, float hitBackSpeed, Weapons.PushType pushtype);

    public void endHurt()
    {
        ChangeState(idleState);
    }

    protected void flip()
    {
        transform.localScale = new Vector3((float)moveModel.Direction, transform.localScale.y, transform.localScale.z);
    }

    protected void checkGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundSensorStartPos.position, Vector2.down, groundHitDis, groundMask);
        Debug.DrawRay(groundSensorStartPos.position, Vector2.down, Color.white);
        if (hit.collider != null)
        {
            moveModel.isGrounded = true;
        }else
        {
            moveModel.isGrounded = false;
        }
    }

    protected void checkPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(eyePos.position, new Vector2((int)moveModel.Direction, 0), sightDis, playerMask);
        Debug.DrawRay(eyePos.position, new Vector2((int)moveModel.Direction, 0), Color.red);
        if (hit.collider != null)
        {
            target = hit.collider.gameObject;
        }
        else
        {
            target = null;
        }
    }
    public void turn()
    {
        if((int)moveModel.Direction == 1)
            moveModel.Direction = EnemyMoveModel.EnemyDirection.Left;
        else
            moveModel.Direction = EnemyMoveModel.EnemyDirection.Right;
    }

    #region coroutines
    public void HitFlash()
    {
        StartCoroutine(HitFlashIE(flashTime));
    }

    IEnumerator HitFlashIE(float duration)
    {
        enemySP.material.SetInt("_Hit", 1);
        yield return new WaitForSeconds(duration);
        enemySP.material.SetInt("_Hit", 0);
    }

    #endregion
}
