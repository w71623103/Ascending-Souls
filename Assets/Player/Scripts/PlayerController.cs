using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public enum state 
    {
        move,
        jump,
        gJump,
        dash,
        slide,
        wallJump,
        attack,
        attackAir,
        grapple,
        attackSP,
        attackUP,
        attackDown,
        hurt,
    }
    public state statevisualizer;

    public PlayerMoveModel moveModel = new PlayerMoveModel();
    public PlayerJumpModel jumpModel = new PlayerJumpModel();
    public PlayerGrappleJumpState grappleJumpState = new PlayerGrappleJumpState();
    public PlayerDashModel dashModel = new PlayerDashModel();
    public PlayerSlideModel slideModel = new PlayerSlideModel();
    public PlayerGrappleModel grappleModel = new PlayerGrappleModel();
    public PlayerAttackModel attackModel = new PlayerAttackModel();
    public PlayerWeaponModel weaponModel = new PlayerWeaponModel();
    public PlayerInteractModel interactModel = new PlayerInteractModel();
    public PlayerHurtModel hurtModel = new PlayerHurtModel();

    public PlayerState generalState;
    public PlayerMoveState moveState = new PlayerMoveState();
    public PlayerJumpState jumpState = new PlayerJumpState();
    public PlayerDashState dashState = new PlayerDashState();
    public PlayerSlidingState slideState = new PlayerSlidingState();
    public PlayerWallJumpState wallJumpState = new PlayerWallJumpState();
    public PlayerGrappleState grappleState = new PlayerGrappleState();
    public PlayerAttackState attackState = new PlayerAttackState();
    public PlayerAttackStateAir attackStateAir = new PlayerAttackStateAir();
    public PlayerAttackSPState attackStateSP = new PlayerAttackSPState();
    public PlayerAttackUPState attackStateUp = new PlayerAttackUPState();
    public PlayerHurtState hurtState = new PlayerHurtState();
    public PlayerAttackDownState attackStateDown = new PlayerAttackDownState();

    [Header("Components")]
    public Rigidbody2D playerRB;
    public Animator playerAnim;
    public SpriteRenderer playerSP;
    public SoundManager_Player soundM;
    public PlayerAmmo ammo;
    public PlayerHeal heal;
    [SerializeField] private MoreMountains.Feedbacks.MMFeedbacks damageFeedback;

    private int groundHash;
    private int jumpVelHash;
    private int extraJumpTriggerHash;

    [Header("LayerMasks")]
    private LayerMask groundMask;

    //Vars for development (should not be serializing after build)
    /*[SerializeField] */private float groundHitDis = 0.28f;
    /*[SerializeField] */private float slideHitDis = 0.1f;
    [SerializeField] private float slideAllowHitDis = 1f;
    /*[SerializeField] */

    public void ChangeState(PlayerState newState)
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
        statevisualizer = state.move;
        playerRB = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        playerSP = GetComponent<SpriteRenderer>();
        //grappleModel.playerLine = GetComponent<LineRenderer>();
        ammo = GetComponent<PlayerAmmo>();
        heal = GetComponent<PlayerHeal>();

        grappleModel.playerLine.enabled = false;
        grappleModel.playerLine.SetPosition(0, transform.position);

        grappleModel.GrappleSensor = transform.Find("GrappleSensor").GetComponent<GrappleArea>();

        interactModel.interactBoxSize = GetComponent<CapsuleCollider2D>().size;

        groundHash = Animator.StringToHash("isGrounded");
        extraJumpTriggerHash = Animator.StringToHash("ExJumpTrigger");
        jumpVelHash = Animator.StringToHash("jumpVelocity");

        groundMask = LayerMask.GetMask("Ground");
        attackModel.attackCountMax = weaponModel.currentWeapon.attackCountMax;
        ChangeState(moveState);
    }

    // Update is called once per frame
    void Update()
    {
        /*weaponModel.WeaponIcon.GetComponent<UnityEngine.UI.Image>().sprite = weaponModel.currentWeapon.icon;*/
        playerAnim.SetBool(groundHash, jumpModel.isGrounded);
        playerAnim.SetFloat(jumpVelHash, playerRB.velocity.y);
        generalState.Update(this);
        checkGround();
        checkSlide();
        flip();
        //reset Platform Collision========================
        if (jumpModel.platformTimer > 0f)
        {
            jumpModel.platformTimer -= Time.deltaTime;
        }
        else
        {
            if(generalState != grappleJumpState)
                gameObject.layer = 6;
        }

        if (generalState != attackStateSP)
        {
            switch (weaponModel.currentWeapon.type)
            {
                case Weapons.WeaponType.BareHand:
                    playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("BareHand"), 1f);
                    playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("Sword"), 0f);
                    playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("GreatSword"), 0f);
                    playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("DualSword"), 0f);
                    break;
                case Weapons.WeaponType.Sword:
                    playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("BareHand"), 0f);
                    playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("Sword"), 1f);
                    playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("GreatSword"), 0f);
                    playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("DualSword"), 0f);
                    break;
                case Weapons.WeaponType.GreatSword:
                    playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("BareHand"), 0f);
                    playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("Sword"), 0f);
                    playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("GreatSword"), 1f);
                    playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("DualSword"), 0f);
                    break;
                case Weapons.WeaponType.DualBlade:
                    playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("BareHand"), 0f);
                    playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("Sword"), 0f);
                    playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("GreatSword"), 0f);
                    playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("DualSword"), 1f);
                    break;
            }
        }

        //weaponModel.WeaponIcon.GetComponent<UnityEngine.UI.Image>().sprite = weaponModel.currentWeapon.icon;

        //Timers
        if (dashModel.dashCDTimer >= 0) dashModel.dashCDTimer -= Time.deltaTime;
        if (slideModel.slidingCancelTimer >= 0) slideModel.slidingCancelTimer -= Time.deltaTime;
        if (attackModel.attackTimer > 0f && generalState != attackState) attackModel.attackTimer -= Time.deltaTime;
        if (attackModel.attackTimerAir > 0f && generalState != attackState) attackModel.attackTimerAir -= Time.deltaTime;
        if (attackModel.comboDropTimer > 0f) attackModel.comboDropTimer -= Time.deltaTime;
        else
        {
            if (attackModel.dropComboBool)
            {
                attackModel.historyAttacCount = 0;
                attackModel.dropComboBool = false;
            }
        }
        if (grappleModel.enemyGrappleTimer >= 0) grappleModel.enemyGrappleTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
         generalState.FixedUpdate(this);
    }

    void OnMove(InputValue input)
    {
        /*if (generalState != dashState)*/
        moveModel.HorizontalMovement = input.Get<Vector2>().x;
        if (generalState != slideState && attackModel.allowTurn)
        {
            if (moveModel.HorizontalMovement < 0f)
                moveModel.Direction = PlayerMoveModel.PlayerDirection.Left;
            else if (moveModel.HorizontalMovement > 0f)
                moveModel.Direction = PlayerMoveModel.PlayerDirection.Right;
        }
        moveModel.VerticalMovement = input.Get<Vector2>().y;
    }

    void OnJump()
    {
        if(generalState != jumpState && jumpModel.jumpCount > 0 && generalState != slideState && generalState != wallJumpState && generalState != attackStateSP && generalState != hurtState && generalState != attackStateDown && generalState != attackStateAir)
        {
            gameObject.layer = 6;
            if (moveModel.VerticalMovement >= 0f)
            {
                jumpModel.jumpCount -= 1;
                playerRB.velocity = Vector2.up * jumpModel.jumpSpeed;
                ChangeState(jumpState);
                
            }
            else
            {
                if(!jumpModel.isPlatform)
                {
                    jumpModel.jumpCount -= 1;
                    playerRB.velocity = Vector2.up * jumpModel.jumpSpeed;
                    ChangeState(jumpState);
                }
                else
                {
                    if (jumpModel.platformTimer <= 0f)
                    {
                        ChangeState(jumpState);
                        gameObject.layer = 7;
                        jumpModel.platformTimer = jumpModel.platformMaxTimer;
                    }
                }
            }
        }
        else if (generalState == slideState)
        {
            jumpModel.jumpCount -= 1;
            ChangeState(wallJumpState);
        }
        else
        {
            if(jumpModel.jumpCount > 0)
            {
                playerRB.velocity = Vector2.up * jumpModel.jumpSpeed;
                jumpModel.jumpCount -= 1;
                playerAnim.SetTrigger(extraJumpTriggerHash);
                
            }
        }
    }

    void OnDash()
    {
        if(dashModel.dashCDTimer <= 0f && generalState != dashState && dashModel.allowDash && generalState != slideState && generalState != attackStateSP)
        {
            ChangeState(dashState);
        }
    }

    void OnQuit()
    {
        Application.Quit();
    }
    
    /*void OnDropWeapon()
    {
        if (*//*jumpModel.isGrounded*//*ammo.percent >= 0.75)
        {
            if (generalState != attackStateSP && generalState != dashState && generalState != wallJumpState && generalState != grappleState && generalState != slideState && generalState != grappleJumpState)
            {
                if (weaponModel.currentWeapon.type != Weapons.WeaponType.BareHand)
                {
                    ammo.decrease(100f);
                    attackModel.allowInput = false;
                    weaponModel.nextWeapon = weaponModel.bareHand;
                    ChangeState(attackStateSP);
                }
            }
        }
    }*/

    public void pickWeapon(Weapons newWeapon)
    {
        if (generalState != attackStateSP && generalState != dashState && generalState != wallJumpState && generalState != grappleState && generalState != slideState && generalState != grappleJumpState)
        {
            //ammo.increase(20f);
            attackModel.allowInput = false;
            weaponModel.nextWeapon = newWeapon;
            ChangeState(attackStateSP);
            /*switch (weaponModel.currentWeapon.type)
            {
                case Weapons.WeaponType.Sword:
                    //Instantiate(weaponModel.swordPick, transform.position, Quaternion.identity);
                    break;
                case Weapons.WeaponType.GreatSword:
                    //Instantiate(weaponModel.greatSwordPick, transform.position, Quaternion.identity);
                    break;
                case Weapons.WeaponType.DualBlade:
                    //Instantiate(weaponModel.dualSwordPick, transform.position, Quaternion.identity);
                    break;
            }*/
            weaponModel.currentWeapon = newWeapon;
        }
    }

    void OnAttack() 
    {
        if (moveModel.VerticalMovement > 0)
        {
            if (attackModel.attackTimer <= 0f && generalState != attackState && generalState != dashState && generalState != attackStateSP && generalState != attackStateUp && generalState != hurtState && generalState != grappleJumpState && generalState != attackStateDown)
                ChangeState(attackStateUp);
            else if (attackModel.allowInput && generalState != attackStateSP && generalState != attackStateUp)
            {
                ChangeState(attackStateUp);
            }
        }
        /*else if(moveModel.VerticalMovement < 0)
        {
            if(!jumpModel.isGrounded)
            {
                if (attackModel.attackTimer <= 0f && generalState != attackState && generalState != dashState && generalState != attackStateSP && generalState != attackStateUp && generalState != hurtState && generalState != grappleJumpState && generalState != attackStateDown)
                    ChangeState(attackStateDown*//*attackState*//*);
                else if (attackModel.allowInput && generalState != attackStateSP && generalState != attackStateUp && generalState != attackStateDown)
                {
                    ChangeState(attackStateDown*//*attackState*//*);
                }
            }
            else //if on ground, down+attack equals a normal attack combo
            {
                if (generalState != attackState && generalState != dashState && generalState != attackStateSP && generalState != attackStateUp && generalState != hurtState && generalState != attackStateDown)
                {
                    if (attackModel.attackTimer <= 0f)
                        ChangeState(attackState);
                }
                else
                {
                    if (attackModel.allowInput && generalState != attackStateSP && generalState != attackStateUp && generalState != grappleJumpState && generalState != attackStateDown)
                    {
                        attackModel.allowInput = false;
                        attackModel.comboed = true;
                        nextCombo();
                    }
                }
            }
            
        }*/
        else if (jumpModel.isGrounded)
        {
            if (generalState != attackState && generalState != dashState && generalState != attackStateSP && generalState != attackStateUp && generalState != hurtState && generalState != attackStateDown)
            {
                if (/*attackModel.attackTimer <= 0f*/true)
                    ChangeState(attackState);
            }
            else
            {
                if (attackModel.allowInput && generalState != attackStateSP && generalState != attackStateUp && generalState != grappleJumpState && generalState != hurtState && generalState != attackStateDown)
                {
                    attackModel.allowInput = false;
                    attackModel.comboed = true;
                    nextCombo();
                }
            }
        }
        else
        {
            if(attackModel.airAttackComboCount > 0)
            {
                if (generalState != attackStateAir && generalState != dashState && generalState != hurtState && generalState != grappleJumpState && generalState != attackStateDown)
                {
                    if (attackModel.attackTimerAir <= 0f)
                        ChangeState(attackStateAir);
                }
                else
                {
                    if (attackModel.allowInput)
                    {
                        attackModel.allowInput = false;
                        attackModel.comboed = true;
                        nextCombo();
                    }
                }
            }
        }
    }

    private void nextCombo()
    {
        if (jumpModel.isGrounded)
        {
            if (attackModel.attackCount < attackModel.attackCountMax)
            {
                attackModel.attackCount++;
            }
            else
            {
                attackModel.attackCount = 1;
            }
        }
        else
        {
            if (attackModel.attackCountAir < attackModel.attackCountAirMax)
            {
                attackModel.attackCountAir++;
            }
            else
            {
                attackModel.attackCountAir = 1;
            }
        }
    }

    void OnGrapple()
    {
        if(generalState != attackState && generalState != dashState && generalState != hurtState && generalState != attackStateSP && generalState != attackStateUp && generalState != attackStateDown)
        {
            Vector3 grappleDir = (grappleModel.point.transform.position - transform.position).normalized;
            float length = Vector3.Distance(transform.position, grappleModel.point.transform.position);
            RaycastHit2D hitGround = Physics2D.Raycast(transform.position, grappleDir, length, groundMask);
            if (hitGround.collider == null && generalState != dashState && grappleModel.point.activeSelf)
            {
                GameObject target = grappleModel.GrappleSensor.closestGrapplePoint;
                if (target != null) StartCoroutine(disableGrapplePoint(target));
                ChangeState(grappleState);
            }
        }
    }

    void OnGrappleE()
    {
        if (generalState != dashState && generalState != hurtState && generalState != slideState && grappleModel.enemyGrappleTimer <= 0f)
        {
            Vector3 grappleDir = (grappleModel.enemyPoint.transform.position - transform.position).normalized;
            float length = Vector3.Distance(transform.position, grappleModel.enemyPoint.transform.position);
            RaycastHit2D hitGround = Physics2D.Raycast(transform.position, grappleDir, length, groundMask);
            if (hitGround.collider == null && generalState != dashState && grappleModel.enemyPoint.activeSelf)
            {
                GameObject target = grappleModel.EnemySensor.closestGrapplePoint;
                if (target != null)
                {
                    grappleModel.enemyGrappleTimer = grappleModel.enemyGrappleTimerMax;
                    target.GetComponent<Enemy>().startGrapple(grappleModel.enemyGrapplePos.position, this);
                }
            }
        }
    }

    void OnWeaponGrapple()
    {
        if (generalState != attackStateSP && generalState != attackState && generalState != dashState && generalState != wallJumpState && generalState != grappleState && generalState != slideState && generalState != hurtState)
        {
            RaycastHit2D hitWeapon = Physics2D.Raycast(grappleModel.WeaponGrappleStartPos.position, new Vector2((int)moveModel.Direction, 0), grappleModel.weaponGrappleLength, LayerMask.GetMask("Interactable"));
            if (hitWeapon.collider != null)
            {
                if(hitWeapon.collider.CompareTag("WeaponPick")) hitWeapon.collider.gameObject.GetComponent<ItemMovement>().activate(grappleModel.WeaponGrappleTargetPos.position);
            }
        }
    }

    void OnInteract()
    {
        if (generalState != attackStateSP && generalState != attackState && generalState != dashState && generalState != wallJumpState && generalState != grappleState && generalState != slideState && generalState != hurtState && generalState != attackStateDown)
        {
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, interactModel.interactBoxSize, 0f, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Interactable"));
            if (hit.collider != null)
            {
                switch (hit.collider.tag)
                {
                    /*case "WeaponPick":
                        hit.collider.GetComponent<WeaponPick>().OnInteract(this);
                        break;*/

                    case "SavePoint":
                        hit.collider.GetComponent<SavePoint>().OnInteract(this);
                        break;

                    default:
                        hit.collider.GetComponent<Interactable>().OnInteract(this);
                        break;
                }
            }
        }
    }

    void OnHeal()
    {
        heal.Heal();
    }

    void OnSwitchFist()
    {
        if(ammo.switchPoint > 0)
        {
            pickWeapon(weaponModel.bareHand);
        }
    }

    void OnSwitchSword()
    {
        if (ammo.switchPoint > 0)
        {
            pickWeapon(weaponModel.sword);
        }
    }

    void OnSwitchDualBlade()
    {
        if (ammo.switchPoint > 0)
        {
            pickWeapon(weaponModel.dualSword);
        }
    }

    void OnSwitchGreatSword()
    {
        if (ammo.switchPoint > 0)
        {
            pickWeapon(weaponModel.greatSword);
        }
    }

    void flip()
    {
        transform.localScale = new Vector3((float)moveModel.Direction, transform.localScale.y, transform.localScale.z);
    }

    void checkGround()
    {
        RaycastHit2D hitJump = Physics2D.Raycast(transform.position + moveModel.groundSensor.transform.localPosition, Vector2.down, groundHitDis, groundMask);
        if (hitJump.collider != null)
        {
            jumpModel.isGrounded = true;
            if (hitJump.collider.CompareTag("Platform")) jumpModel.isPlatform = true;
            dashModel.allowDash = true;
            attackModel.airAttackComboCount = attackModel.airAttackComboCountMax;
            //if (generalState != attackState) attackModel.attackCount = 0;
            if(playerRB.gravityScale != slideModel.normalGravity) playerRB.gravityScale = slideModel.normalGravity;
            if (playerRB.velocity.y < 0f && generalState != dashState && generalState != moveState && generalState != attackState && generalState != attackStateAir && generalState != grappleState && generalState != attackStateSP && generalState != hurtState && generalState != attackStateDown)
            { 
                ChangeState(moveState);
                jumpModel.jumpCount = jumpModel.jumpCountMax;
            }
        }
        else
        {
            jumpModel.isGrounded = false;
        }

        RaycastHit2D hitSlide = Physics2D.Raycast(transform.position + moveModel.groundSensor.transform.localPosition, Vector2.down, slideAllowHitDis, groundMask);
        if (hitSlide.collider != null)
        {
            slideModel.canSlide = false;
        }
        else
        {
            slideModel.canSlide = true;
        }
    }

    public void checkEnemy()
    {
        RaycastHit2D hitJump = Physics2D.Raycast(transform.position + moveModel.groundSensor.transform.localPosition, Vector2.down, groundHitDis, LayerMask.GetMask("Enemy"));
        if (hitJump.collider != null)
        {
            Physics2D.IgnoreLayerCollision(6, 14, true);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(6, 14, false);
        }
    }

    void checkSlide()
    {
        Vector3 HitSlideStart = transform.position +
            new Vector3(slideModel.slideDetectPos.transform.localPosition.x * (int)moveModel.Direction,
            slideModel.slideDetectPos.transform.localPosition.y,
            0);
        RaycastHit2D hit = Physics2D.Raycast(HitSlideStart,
            new Vector2((int)moveModel.Direction, 0), slideHitDis, groundMask);
        if (hit.collider != null && hit.collider.CompareTag("Slidable") && slideModel.canSlide && slideModel.slidingCancelTimer <= 0f && generalState != slideState && generalState != attackStateSP && generalState != attackStateAir && generalState != attackStateDown)
        {
            attackModel.airAttackComboCount = attackModel.airAttackComboCountMax;
            ChangeState(slideState);
        }
        else if (hit.collider == null && generalState != jumpState && generalState != dashState && generalState != wallJumpState && generalState != attackState && generalState != grappleState && generalState != attackStateSP && generalState != grappleJumpState && generalState != hurtState && generalState != attackStateAir && generalState != attackStateDown)
        {
            ChangeState(moveState);
        }
    }

    IEnumerator disableGrapplePoint(GameObject point)
    {
        point.SetActive(false);
        yield return new WaitForSecondsRealtime(grappleModel.grapplePointExcludeCD);
        point.SetActive(true);
    }

    public void OnHit(Vector2 hitBackDir, float hitBackSpeed, Weapons.PushType pushtype)
    {
        damageFeedback?.PlayFeedbacks();
        switch (pushtype)
        {
            case Weapons.PushType.pushBack:
                playerRB.gravityScale = 3f;
                playerRB.velocity = hitBackDir * hitBackSpeed;
                break;
            case Weapons.PushType.upKa:
                playerRB.gravityScale = 2f;
                playerRB.velocity = hitBackDir * hitBackSpeed;
                break;
            case Weapons.PushType.quickFall:
                playerRB.gravityScale = 10f;
                break;
            case Weapons.PushType.inPlace:
                playerRB.gravityScale = 0f;
                playerRB.velocity = Vector2.zero;
                //enemyRB.velocity = new Vector2(0f, hitBackDir.y * hitBackSpeed);
                break;
        }

        //play sound
        soundM.playHit();
        //shader flash
        HitFlash();
        if(generalState != dashState && generalState != attackStateSP)
            ChangeState(hurtState);
    }

    public void HitFlash()
    {
        StartCoroutine(HitFlashIE(hurtModel.flashTime));
    }

    IEnumerator HitFlashIE(float duration)
    {
        playerSP.material.SetInt("_Hit", 1);
        playerSP.material.SetInt("_HitBlack", 1);
        yield return new WaitForSeconds(duration * 0.5f);
        playerSP.material.SetInt("_HitBlack", 0);
        yield return new WaitForSeconds(duration);
        playerSP.material.SetInt("_Hit", 0);
    }

    public void DropCombo()
    {
        /*StartCoroutine(DropComboCoro());*/
        attackModel.historyAttacCount = attackModel.attackCount;
        attackModel.attackCount = 0;
        attackModel.comboDropTimer = attackModel.comboDropTime;
        attackModel.dropComboBool = true;
    }

    /*IEnumerator DropComboCoro()
    {
        Debug.Log("1");
        yield return new WaitForSecondsRealtime(attackModel.comboDropTime);
        Debug.Log("2");
        attackModel.attackCount = 0;
    }*/
}
