using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public enum enemyStates
    {
        Idle,
        Hurt,
    }
    public EnemyMoveModel moveModel = new EnemyMoveModel();

    public enemyStates statevisualizer;
    [Header("Component")]
    [SerializeField] public Rigidbody2D enemyRB;
    [SerializeField] public Animator enemyAnim;
    [SerializeField] public SpriteRenderer enemySP;
    [SerializeField] public SoundManager_Enemy soundM;
    [SerializeField] protected Transform groundSensorStartPos;
    [SerializeField] protected Hp myHp;

    [Header("Hit")]
    [SerializeField] protected bool allowHitRecover = true;
    [SerializeField] public Vector2 hitDir;
    [SerializeField] public float flashTime = 0.1f;
    //[SerializeField] protected GameObject blood;

    [Header("ground check")]
    [SerializeField] protected float groundHitDis = 0.2f;
    protected LayerMask groundMask;


    public EnemyState generalState;
    public EnemyIdleState idleState;
    public EnemyHurtState hurtState;

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

    // Start is called before the first frame update
    void Start()
    {
        groundMask = LayerMask.GetMask("Ground");
        enemyRB = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
        enemySP = GetComponent<SpriteRenderer>();

        ChangeState(idleState);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public abstract void OnHit(Vector2 hitBackDir, float hitBackSpeed, int damage, Weapons.PushType pushtype);

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

    public void turn()
    {
        if((int)moveModel.Direction == 1)
            moveModel.Direction = EnemyMoveModel.EnemyDirection.Left;
        else
            moveModel.Direction = EnemyMoveModel.EnemyDirection.Right;
    }

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
}
