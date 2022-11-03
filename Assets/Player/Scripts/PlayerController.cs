using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        grapple,
        attackSP,
        attackUP,
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

    public PlayerState generalState;
    public PlayerMoveState moveState = new PlayerMoveState();
    public PlayerJumpState jumpState = new PlayerJumpState();
    public PlayerDashState dashState = new PlayerDashState();
    public PlayerSlidingState slideState = new PlayerSlidingState();
    public PlayerWallJumpState wallJumpState = new PlayerWallJumpState();
    public PlayerGrappleState grappleState = new PlayerGrappleState();
    public PlayerAttackState attackState = new PlayerAttackState();
    public PlayerAttackSPState attackStateSP = new PlayerAttackSPState();
    public PlayerAttackUPState attackStateUp = new PlayerAttackUPState();

    [Header("Components")]
    public Rigidbody2D playerRB;
    public Animator playerAnim;

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
        grappleModel.playerLine = GetComponent<LineRenderer>();
        grappleModel.playerLine.enabled = false;
        grappleModel.playerLine.SetPosition(0, transform.position);

        grappleModel.GrappleSensor = transform.Find("GrappleSensor").GetComponent<GrappleArea>();

        interactModel.interactBoxSize = GetComponent<CapsuleCollider2D>().size;

        groundHash = Animator.StringToHash("isGrounded");
        extraJumpTriggerHash = Animator.StringToHash("ExJumpTrigger");
        jumpVelHash = Animator.StringToHash("jumpVelocity");

        groundMask = LayerMask.GetMask("Ground");
        ChangeState(moveState);
    }

    // Update is called once per frame
    void Update()
    {
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
                    break;
                case Weapons.WeaponType.Sword:
                    playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("BareHand"), 0f);
                    playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("Sword"), 1f);
                    playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("GreatSword"), 0f);
                    break;
                case Weapons.WeaponType.GreatSword:
                    playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("BareHand"), 0f);
                    playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("Sword"), 0f);
                    playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("GreatSword"), 1f);
                    break;
            }
        }
        //Timers
        if (slideModel.slidingCancelTimer >= 0) slideModel.slidingCancelTimer -= Time.deltaTime;
        if (attackModel.attackTimer > 0f && generalState != attackState) attackModel.attackTimer -= Time.deltaTime;
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
        if(generalState != jumpState && jumpModel.jumpCount > 0 && generalState != slideState && generalState != wallJumpState && generalState != attackStateSP)
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
        if(generalState != dashState && dashModel.allowDash && generalState != slideState && generalState != attackStateSP)
        {
            ChangeState(dashState);
        }
    }

    void OnQuit()
    {
        Application.Quit();
    }
    
    void OnDropWeapon()
    {
        if (/*jumpModel.isGrounded*/true)
        {
            if (generalState != attackStateSP && generalState != dashState && generalState != wallJumpState && generalState != grappleState && generalState != slideState /*&& generalState != hurtState*/)
            {
                if (weaponModel.currentWeapon.type != Weapons.WeaponType.BareHand)
                {
                    /*switch(weaponModel.currentWeapon.type)
                    {
                        case Weapons.WeaponType.Sword:
                            Instantiate(weaponModel.swordPick,transform.position,Quaternion.identity);
                            break;
                        default:
                            break;
                    }*/
                    attackModel.allowInput = false;
                    weaponModel.nextWeapon = weaponModel.bareHand;
                    ChangeState(attackStateSP);
                }
            }
        }
    }

    public void pickWeapon(Weapons newWeapon)
    {
        if (/*jumpModel.isGrounded*/true)
        {
            if (generalState != attackStateSP && generalState != attackState && generalState != dashState && generalState != wallJumpState && generalState != grappleState && generalState != slideState/*&& generalState != hurtState*/)
            {
                attackModel.allowInput = false;
                weaponModel.nextWeapon = newWeapon;
                ChangeState(attackStateSP);
            }
        }
    }

    void OnAttack() 
    {
        if (moveModel.VerticalMovement > 0)
        {
            if (attackModel.attackTimer <= 0f && generalState != attackState && generalState != dashState && generalState != attackStateSP && generalState != attackStateUp/*&& generalState != hurtState*/)
                ChangeState(attackStateUp);
        }
        else if (jumpModel.isGrounded)
        {
            if (generalState != attackState && generalState != dashState && generalState != attackStateSP && generalState != attackStateUp/*&& generalState != hurtState*/)
            {
                if (attackModel.attackTimer <= 0f)
                    ChangeState(attackState);
            }
            else
            {
                if (attackModel.allowInput && generalState != attackStateSP && generalState != attackStateUp/* && stemina > 0*/)
                {
                    if (moveModel.VerticalMovement > 0)
                    {
                        ChangeState(attackStateUp);
                    }
                    else
                    {
                        attackModel.allowInput = false;
                        attackModel.comboed = true;
                        nextCombo();
                    }
                }
            }
        }
        else
        {
            if(attackModel.airAttackCount > 0)
            {
                if (generalState != attackState && generalState != dashState /*&& generalState != hurtState*/)
                {
                    if (attackModel.attackTimer <= 0f/* && stemina > 0*/)
                        ChangeState(attackState);
                }
                else
                {
                    if (attackModel.allowInput/* && stemina > 0*/)
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
        if(attackModel.attackCount < attackModel.attackCountMax)
        {
            attackModel.attackCount++;
        }else
        {
            attackModel.attackCount = 1;
        }
    }

    void OnGrapple()
    {
        Vector3 grappleDir = (grappleModel.point.transform.position - transform.position).normalized;
        float length = Vector3.Distance(transform.position, grappleModel.point.transform.position);
        RaycastHit2D hitGround = Physics2D.Raycast(transform.position, grappleDir, length, groundMask);
        if (hitGround.collider == null && generalState != dashState && grappleModel.point.activeSelf)
        {
            GameObject target = grappleModel.GrappleSensor.closestGrapplePoint;
            if(target != null) StartCoroutine(disableGrapplePoint(target));
            ChangeState(grappleState);
        }
    }

    void OnInteract()
    {
        if (generalState != attackStateSP && generalState != attackState && generalState != dashState && generalState != wallJumpState && generalState != grappleState && generalState != slideState/*&& generalState != hurtState*/)
        {
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, interactModel.interactBoxSize, 0f, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Interactable"));
            if (hit.collider != null)
            {
                switch (hit.collider.tag)
                {
                    case "WeaponPick":
                        hit.collider.GetComponent<WeaponPick>().OnInteract(this);
                        break;

                    default:

                        break;
                }
            }
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
            attackModel.airAttackCount = attackModel.airAttackCountMax;
            if (generalState != attackState) attackModel.attackCount = 0;
            if(playerRB.gravityScale != slideModel.normalGravity) playerRB.gravityScale = slideModel.normalGravity;
            if (playerRB.velocity.y < 0f && generalState != dashState && generalState != moveState && generalState != attackState && generalState != grappleState && generalState != attackStateSP)
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

    void checkSlide()
    {
        Vector3 HitSlideStart = transform.position +
            new Vector3(slideModel.slideDetectPos.transform.localPosition.x * (int)moveModel.Direction,
            slideModel.slideDetectPos.transform.localPosition.y,
            0);
        RaycastHit2D hit = Physics2D.Raycast(HitSlideStart,
            new Vector2((int)moveModel.Direction, 0), slideHitDis, groundMask);
        if (hit.collider != null && hit.collider.CompareTag("Slidable") && slideModel.canSlide && slideModel.slidingCancelTimer <= 0f && generalState != slideState)
        {
            attackModel.airAttackCount = attackModel.airAttackCountMax;
            ChangeState(slideState);
        }
        else if (hit.collider == null && generalState != jumpState && generalState != dashState && generalState != wallJumpState && generalState != attackState && generalState != grappleState && generalState != attackStateSP && generalState != grappleJumpState)
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
}
