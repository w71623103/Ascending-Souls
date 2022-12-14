using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMan : Enemy
{
    #region FSM
    public SwordManStateBase generalState;
    public SwordManIdleState idleState = new SwordManIdleState();
    public SwordManHurtState hurtState = new SwordManHurtState();
    public SwordManAttackState attackState = new SwordManAttackState();
    public SwordManAttackSPState attackStateSP = new SwordManAttackSPState();
    public SwordManPatrolState patrolState = new SwordManPatrolState();
    public SwordManChaseState chaseState = new SwordManChaseState();
    public SwordManAttackFinishState attackFinishState = new SwordManAttackFinishState();

    public void ChangeState(SwordManStateBase newState)
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

    #region Models
    public EnemyMoveModel moveModel = new EnemyMoveModel();
    public EnemyIdleModel idleModel = new EnemyIdleModel();
    public EnemyPatrolModel patrolModel = new EnemyPatrolModel();
    public EnemyAttackModel attackModel = new EnemyAttackModel();
    public EnemyChaseModel chaseModel = new EnemyChaseModel();
    #endregion

    [SerializeField] private GameObject hateBar;
    [SerializeField] private HitRecoverTolerance hitRT;
    // Start is called before the first frame update
    void Start()
    {
        groundMask = LayerMask.GetMask("Ground");
        playerMask = LayerMask.GetMask("PlayerCol");

        enemyRB = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
        enemySP = GetComponent<SpriteRenderer>();
        soundM = GetComponent<SoundManager_Enemy>();
        myHp = GetComponent<Hp>();
        hitRT = GetComponent<HitRecoverTolerance>();

        ChangeState(idleState);
    }

    private void Update()
    {
        testUI();
        generalState.Update(this);
        flip();
        checkGround();
        checkPlayer();
        attackModel.hatePercent = attackModel.attackSPHate / attackModel.attackSPHateGate;
    }

    private void FixedUpdate()
    {
        generalState.FixedUpdate(this);
    }

    public override void OnHit(int damage, Vector2 hitBackDir, float hitBackSpeed, Weapons.PushType pushtype, float hitRecover)
    {
        damageFeedback?.PlayFeedbacks(this.transform.position, damage);
        hitDir = hitBackDir;
        switch(pushtype)
        {
            case Weapons.PushType.pushBack:
                enemyRB.gravityScale = 3f;
                enemyRB.velocity = hitBackDir * hitBackSpeed;
                break;
            case Weapons.PushType.upKa:
                enemyRB.gravityScale = 2f;
                enemyRB.velocity = hitBackDir * hitBackSpeed;
                break;
            case Weapons.PushType.quickFall:
                enemyRB.gravityScale = 10f;
                break;
            case Weapons.PushType.inPlace:
                enemyRB.gravityScale = 0f;
                enemyRB.velocity = Vector2.zero;
                //enemyRB.velocity = new Vector2(0f, hitBackDir.y * hitBackSpeed);
                break;
        }
        
        //play sound
        soundM.playHit();
        //shader flash
        HitFlash();
        //Instantiate(blood,transform.position, Quaternion.identity)

        //
        if (hitRecover > hitRT.num)
        {
            ChangeState(hurtState);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(eyePos.position, new Vector3((int)moveModel.Direction, 0, 0));
        Gizmos.DrawWireSphere(attackModel.attackPointNormal.position, attackModel.attackNormalRange);
        Gizmos.DrawWireSphere(attackModel.attackPointSP.position, attackModel.attackSPRange);
    }

    private void testUI()
    {
        if(hateBar != null && attackModel.attackSPHate <= attackModel.attackSPHateGate)
            hateBar.transform.localScale = new Vector3(1, attackModel.attackSPHate/attackModel.attackSPHateGate,1);
        else if(hateBar != null && attackModel.attackSPHate > attackModel.attackSPHateGate)
            hateBar.transform.localScale = new Vector3(1, 1, 1);
    }

    protected override void flip()
    {
        transform.localScale = new Vector3((float)moveModel.Direction, transform.localScale.y, transform.localScale.z);
    }

    protected override void checkGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundSensorStartPos.position, Vector2.down, groundHitDis, groundMask);
        Debug.DrawRay(groundSensorStartPos.position, Vector2.down, Color.white);
        if (hit.collider != null)
        {
            moveModel.isGrounded = true;
        }
        else
        {
            moveModel.isGrounded = false;
        }
    }

    protected override void checkPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(eyePos.position, new Vector2((int)moveModel.Direction, 0), sightDis, playerMask);
        Debug.DrawRay(eyePos.position, new Vector2((int)moveModel.Direction, 0), Color.red);
        if (hit.collider != null)
        {
            target = hit.collider.gameObject;
            attackModel.playerInRange = true;
        }
        else
        {
            target = null;
            RaycastHit2D hitBack = Physics2D.Raycast(eyePos.position, new Vector2(-1 * (int)moveModel.Direction, 0), sightDis, playerMask);
            Debug.DrawRay(eyePos.position, new Vector2(-1 * (int)moveModel.Direction, 0), Color.red);
            if(hitBack.collider != null)
            {
                attackModel.playerInRange = true;
            }
            else
            {
                attackModel.playerInRange = false;
            }
        }
    }

    public override void turn()
    {
        if ((int)moveModel.Direction == 1)
            moveModel.Direction = EnemyMoveModel.EnemyDirection.Left;
        else
            moveModel.Direction = EnemyMoveModel.EnemyDirection.Right;
    }
}
