using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMan : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        //States
        idleState = new SwordManIdleState();
        hurtState = new SwordManHurtState();
        attackState = new SwordManAttackState();
        patrolState = new SwordManPatrolState();
        chaseState = new SwordManChaseState();
        groundMask = LayerMask.GetMask("Ground");
        playerMask = LayerMask.GetMask("PlayerCol");

        enemyRB = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
        enemySP = GetComponent<SpriteRenderer>();
        soundM = GetComponent<SoundManager_Enemy>();
        myHp = GetComponent<Hp>();

        ChangeState(idleState);
    }

    private void Update()
    {
        generalState.Update(this);
        flip();
        checkGround();
        checkPlayer();
    }

    public override void OnHit(Vector2 hitBackDir, float hitBackSpeed, Weapons.PushType pushtype)
    {
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
        if (allowHitRecover)
        {
            ChangeState(hurtState);
        }
    }

    
}
