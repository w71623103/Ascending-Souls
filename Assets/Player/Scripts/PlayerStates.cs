using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    public abstract void EnterState(PlayerController pl);
    public abstract void Update(PlayerController pl);
    public abstract void FixedUpdate(PlayerController pl);
    public abstract void ExitState(PlayerController pl);
}

public class PlayerMoveState : PlayerState
{
    private int moveBoolHash = Animator.StringToHash("isMoving");
    public override void EnterState(PlayerController pl)
    {
        pl.statevisualizer = PlayerController.state.move;
        pl.jumpModel.jumpCount = pl.jumpModel.jumpCountMax;
        if (pl.moveModel.HorizontalMovement < 0f)
            pl.moveModel.Direction = PlayerMoveModel.PlayerDirection.Left;
        else if (pl.moveModel.HorizontalMovement > 0f)
            pl.moveModel.Direction = PlayerMoveModel.PlayerDirection.Right;
    }
    public override void Update(PlayerController pl)
    {
        pl.playerAnim.SetBool(moveBoolHash, pl.moveModel.HorizontalMovement != 0f);
        if(pl.playerRB.gravityScale == 0f) pl.playerRB.gravityScale = 3f;
    }
    public override void FixedUpdate(PlayerController pl)
    {
        pl.playerRB.velocity = new Vector2(pl.moveModel.HorizontalMovement * pl.moveModel.hspeed, pl.playerRB.velocity.y);
    }
    public override void ExitState(PlayerController pl)
    {

    }
}

public class PlayerJumpState : PlayerState
{
    public override void EnterState(PlayerController pl)
    {
        /*if(!pl.slideModel.wallJumped)
            pl.jumpModel.jumpCount -= 1;*/
        pl.statevisualizer = PlayerController.state.jump;
    }
    public override void Update(PlayerController pl)
    {
        pl.checkEnemy();
        if (pl.jumpModel.isGrounded && Mathf.Abs(pl.playerRB.velocity.y) < 0.1f) pl.ChangeState(pl.moveState);
    }
    public override void FixedUpdate(PlayerController pl)
    {
        pl.playerRB.velocity = new Vector2(pl.moveModel.HorizontalMovement * pl.moveModel.hspeed, pl.playerRB.velocity.y);
    }
    public override void ExitState(PlayerController pl)
    {
        //pl.jumpModel.jumpCount = pl.jumpModel.jumpCountMax;
    }
}

public class PlayerGrappleJumpState : PlayerState
{
    public override void EnterState(PlayerController pl)
    {
        /*if(!pl.slideModel.wallJumped)
            pl.jumpModel.jumpCount -= 1;*/
        pl.statevisualizer = PlayerController.state.gJump;
        pl.gameObject.layer = 7;
    }
    public override void Update(PlayerController pl)
    {
        if (pl.jumpModel.isGrounded && Mathf.Abs(pl.playerRB.velocity.y) < 0.1f) pl.ChangeState(pl.moveState);
    }
    public override void FixedUpdate(PlayerController pl)
    {
        //pl.playerRB.velocity = new Vector2(pl.moveModel.HorizontalMovement * pl.moveModel.hspeed, pl.playerRB.velocity.y);
    }
    public override void ExitState(PlayerController pl)
    {
        //pl.jumpModel.jumpCount = pl.jumpModel.jumpCountMax;
        pl.gameObject.layer = 6;
    }
}

public class PlayerDashState : PlayerState
{
    private int dashTriggerHash = Animator.StringToHash("dashTrigger");
    private float timer;
    public override void EnterState(PlayerController pl)
    {
        timer = 0f;
        //Play Animation
        pl.playerAnim.SetTrigger(dashTriggerHash);
        pl.statevisualizer = PlayerController.state.dash;
        pl.playerRB.velocity = Vector2.zero;
        pl.playerRB.velocity = new Vector2(pl.dashModel.dashSpeed, 0) * (int)pl.moveModel.Direction;
        if (!pl.jumpModel.isGrounded)
        {
            pl.playerRB.gravityScale = 0f;
            pl.dashModel.allowDash = false;
            if (pl.attackModel.airAttackComboCount < pl.attackModel.airAttackComboCountMax)
                pl.attackModel.airAttackComboCount++;
        }
        Physics2D.IgnoreLayerCollision(6, 14, true);
        Physics2D.IgnoreLayerCollision(6, 16, true);
        Physics2D.IgnoreLayerCollision(7, 16, true);
    }
    public override void Update(PlayerController pl)
    {
        timer += Time.deltaTime;
        if(timer > .5f)
            pl.ChangeState(pl.moveState);
        /*int currentLayer = 0;
        switch(pl.weaponModel.currentWeapon.type)
        {
            case Weapons.WeaponType.BareHand:
                currentLayer = 1;
                break;
            case Weapons.WeaponType.Sword:
                currentLayer = 2;
                break;
            case Weapons.WeaponType.Colossal:
                currentLayer = 3;
                break;
        }
        if (pl.playerAnim.GetCurrentAnimatorStateInfo(currentLayer).normalizedTime > .95f)
            pl.ChangeState(pl.moveState);*/
    }
    public override void FixedUpdate(PlayerController pl)
    {
        pl.playerRB.velocity = new Vector2(pl.dashModel.dashSpeed, pl.playerRB.velocity.y) * (int)pl.moveModel.Direction;
    }
    public override void ExitState(PlayerController pl)
    {
        timer = 0f;
        pl.playerRB.velocity = Vector2.zero;
        pl.playerRB.gravityScale = 3f;
        Physics2D.IgnoreLayerCollision(6, 14, false);
        Physics2D.IgnoreLayerCollision(6, 16, false);
        Physics2D.IgnoreLayerCollision(7, 16, false);
        pl.dashModel.dashCDTimer = pl.dashModel.dashCD;
    }
}

public class PlayerSlidingState : PlayerState
{
    public override void EnterState(PlayerController pl)
    {
        pl.jumpModel.jumpCount = pl.jumpModel.jumpCountMax;
        pl.statevisualizer = PlayerController.state.slide;
        pl.playerRB.gravityScale = pl.slideModel.slideGravity;
        pl.playerRB.velocity = Vector2.zero;
        //pl.slideModel.isSliding = true;
        pl.playerAnim.SetBool("isSlide", true);
        //pl.slideModel.slidingCancelTimer = pl.slideModel.slidingCancelTimerMax;
        pl.dashModel.allowDash = true;
    }
    public override void Update(PlayerController pl)
    {
        /*if(pl.slideModel.slidingCancelTimer > 0f)
            pl.slideModel.slidingCancelTimer -= Time.deltaTime;*/
        //pl.playerRB.gravityScale = Mathf.Lerp(0f, pl.slideModel.normalGravityScale, pl.slideModel.slidingCancelTimer);
    }
    public override void FixedUpdate(PlayerController pl)
    {

    }
    public override void ExitState(PlayerController pl)
    {
        //pl.moveModel.HorizontalMovement = 0f;
        pl.playerRB.gravityScale = pl.slideModel.normalGravity;
        pl.playerAnim.SetBool("isSlide", false);

    }
}

public class PlayerWallJumpState : PlayerState
{
    public override void EnterState(PlayerController pl)
    {
        pl.slideModel.slidingCancelTimer = pl.slideModel.slidingCancelTimerMax;
        /*pl.jumpModel.jumpCount--;*/
        pl.statevisualizer = PlayerController.state.wallJump;
        pl.playerRB.velocity = new Vector2((int)pl.moveModel.Direction * -1f * pl.slideModel.slideJumpHorizontalSpeed, pl.jumpModel.jumpSpeed);
        pl.slideModel.wallJumped = true;
    }
    public override void Update(PlayerController pl)
    {
        if (pl.slideModel.slidingCancelTimer < 0f) pl.ChangeState(pl.jumpState);
    }

    public override void FixedUpdate(PlayerController pl)
    {
        
    }

    public override void ExitState(PlayerController pl)
    {
        if (pl.moveModel.HorizontalMovement < 0f)
            pl.moveModel.Direction = PlayerMoveModel.PlayerDirection.Left;
        else if (pl.moveModel.HorizontalMovement > 0f)
            pl.moveModel.Direction = PlayerMoveModel.PlayerDirection.Right;
    }
}

public class PlayerAttackState : PlayerState
{
    public override void EnterState(PlayerController pl)
    {
        pl.statevisualizer = PlayerController.state.attack;
        // start attack
        if (pl.jumpModel.isGrounded && pl.attackModel.historyAttacCount == 0) pl.attackModel.attackCount = 1;
        else
        { 
            if (pl.attackModel.historyAttacCount < pl.attackModel.attackCountMax) pl.attackModel.attackCount = pl.attackModel.historyAttacCount + 1; 
            else pl.attackModel.attackCount = 1;
        }
        /*if (pl.jumpModel.isGrounded) pl.playerAnim.SetInteger("AttackLight", 1);
        else */pl.playerAnim.SetInteger("AttackLight", pl.attackModel.attackCount);
        pl.playerRB.velocity = Vector2.zero;
    }

    public override void Update(PlayerController pl)
    { }

    public override void FixedUpdate(PlayerController pl)
    { }

    public override void ExitState(PlayerController pl)
    {
        pl.playerAnim.SetInteger("AttackLight", 0);
        if(pl.jumpModel.isGrounded) pl.DropCombo() /*pl.attackModel.attackCount = 0*/;
        pl.attackModel.attackTimer = pl.attackModel.attackTimerMax;
    }
}

public class PlayerAttackStateAir : PlayerState
{
    public override void EnterState(PlayerController pl)
    {
        pl.statevisualizer = PlayerController.state.attackAir;
        // start attack
        if (pl.attackModel.attackCountAir < pl.attackModel.attackCountAirMax) pl.attackModel.attackCountAir++;
        else pl.attackModel.attackCountAir = 1;
        pl.playerAnim.SetInteger("AttackLight", pl.attackModel.attackCountAir);
        pl.playerRB.velocity = Vector2.zero;
    }

    public override void Update(PlayerController pl)
    { }

    public override void FixedUpdate(PlayerController pl)
    { }

    public override void ExitState(PlayerController pl)
    {
        pl.playerAnim.SetInteger("AttackLight", 0);
        if (pl.jumpModel.isGrounded) pl.attackModel.attackCountAir = 0;
        pl.attackModel.attackTimerAir = pl.attackModel.attackTimerAirMax;
    }
}

public class PlayerAttackSPState : PlayerState
{
    public override void EnterState(PlayerController pl)
    {
        pl.statevisualizer = PlayerController.state.attackSP;
        // start attack
        pl.playerAnim.SetTrigger("WeaponOut");
        /*        pl.playerRB.velocity = Vector2.zero;*/
        pl.attackModel.allowInput = false;
    }

    public override void Update(PlayerController pl)
    {
        switch(pl.weaponModel.currentWeapon.type)
        {
            case Weapons.WeaponType.BareHand:
                if (!((pl.playerAnim.GetCurrentAnimatorStateInfo(pl.fistLayerId).IsName("In")) || (pl.playerAnim.GetCurrentAnimatorStateInfo(pl.fistLayerId).IsName("out"))))
                {
                    pl.ChangeState(pl.moveState);
                }
                break;

            case Weapons.WeaponType.Sword:
                if (!((pl.playerAnim.GetCurrentAnimatorStateInfo(pl.swordLayerId).IsName("In")) || (pl.playerAnim.GetCurrentAnimatorStateInfo(pl.fistLayerId).IsName("out"))))
                {
                    pl.ChangeState(pl.moveState);
                }
                break;

            case Weapons.WeaponType.GreatSword:
                if (!((pl.playerAnim.GetCurrentAnimatorStateInfo(pl.greatSwordLayerId).IsName("In")) || (pl.playerAnim.GetCurrentAnimatorStateInfo(pl.fistLayerId).IsName("out"))))
                {
                    pl.ChangeState(pl.moveState);
                }
                break;

            case Weapons.WeaponType.DualBlade:
                if (!((pl.playerAnim.GetCurrentAnimatorStateInfo(pl.dualSwordLayerId).IsName("In")) || (pl.playerAnim.GetCurrentAnimatorStateInfo(pl.fistLayerId).IsName("out"))))
                {
                    pl.ChangeState(pl.moveState);
                }
                break;

        }
        
    }

    public override void FixedUpdate(PlayerController pl)
    {
        pl.playerRB.velocity = new Vector2(pl.playerRB.velocity.x, 0f);
    }

    public override void ExitState(PlayerController pl)
    {
        //pl.canInput = false;
        //pl.playerAnim.SetInteger(pl.attackLightHash, 0);
        pl.playerAnim.SetInteger("AttackLight", 0);
        if (pl.jumpModel.isGrounded) pl.attackModel.attackCount = 0;
        pl.attackModel.attackTimer = pl.attackModel.attackTimerMax;
        //pl.playerRB.gravityScale = pl.slideModel.normalGravity;
    }
}

public class PlayerAttackUPState : PlayerState
{
    public override void EnterState(PlayerController pl)
    {
        pl.statevisualizer = PlayerController.state.attackUP;
        // start attack
        pl.playerAnim.SetTrigger("AttackUp");
        /*        pl.playerRB.velocity = Vector2.zero;*/
        pl.attackModel.allowInput = false;
    }

    public override void Update(PlayerController pl)
    { }

    public override void FixedUpdate(PlayerController pl)
    {
        pl.playerRB.velocity = new Vector2(pl.playerRB.velocity.x, 0f);
    }

    public override void ExitState(PlayerController pl)
    {
        //pl.canInput = false;
        //pl.playerAnim.SetInteger(pl.attackLightHash, 0);
        pl.playerAnim.SetInteger("AttackLight", 0);
        if (pl.jumpModel.isGrounded) pl.attackModel.attackCount = 0;
        pl.attackModel.attackTimer = pl.attackModel.attackTimerMax;
        //pl.playerRB.gravityScale = pl.slideModel.normalGravity;
    }
}

public class PlayerAttackDownState : PlayerState
{
    public override void EnterState(PlayerController pl)
    {
        pl.statevisualizer = PlayerController.state.attackUP;
        // start attack
        pl.playerAnim.SetTrigger("AttackDown");
        pl.attackModel.allowInput = false;
    }

    public override void Update(PlayerController pl)
    { }

    public override void FixedUpdate(PlayerController pl)
    {
        //pl.playerRB.velocity = new Vector2(pl.playerRB.velocity.x, 0f);
    }

    public override void ExitState(PlayerController pl)
    {
        pl.playerAnim.SetInteger("AttackLight", 0);
        if (pl.jumpModel.isGrounded) pl.attackModel.attackCount = 0;
        pl.attackModel.attackTimer = pl.attackModel.attackTimerMax;
    }
}


public class PlayerGrappleState : PlayerState
{
    private Vector3 GrappleDirection;
    public override void EnterState(PlayerController pl)
    {
        pl.statevisualizer = PlayerController.state.grapple;
        pl.dashModel.allowDash = true;
        pl.jumpModel.jumpCount = pl.jumpModel.jumpCountMax;
        pl.grappleModel.playerLine.enabled = true;
        pl.grappleModel.playerLine.SetPosition(0, pl.transform.position);
        pl.grappleModel.playerLine.SetPosition(1, pl.grappleModel.point.transform.position);
        
    }
    public override void Update(PlayerController pl)
    {
        pl.grappleModel.playerLine.SetPosition(0, pl.transform.position);
        if (Vector3.Distance(pl.transform.position, pl.grappleModel.point.transform.position) > pl.grappleModel.reachTolerance)
        {
            pl.playerRB.gravityScale = 0f;
        }
        else
        {
            if (pl.jumpModel.isGrounded)
            {
                pl.ChangeState(pl.moveState);
            }
            else
            {
                pl.ChangeState(pl.grappleJumpState);
            }
        }
    }
    public override void FixedUpdate(PlayerController pl)
    {
        if (Vector3.Distance(pl.transform.position, pl.grappleModel.point.transform.position) > pl.grappleModel.directionChangeTolerance) GrappleDirection = Vector3.Normalize((pl.grappleModel.point.transform.position - pl.transform.position));
        pl.playerRB.velocity = new Vector2(GrappleDirection.x, GrappleDirection.y) * pl.grappleModel.grappleJumpSpeed;
    }
    public override void ExitState(PlayerController pl)
    {
        pl.grappleModel.playerLine.enabled = false;
        pl.playerRB.gravityScale = 3f;
        pl.playerRB.velocity = new Vector2(GrappleDirection.x, GrappleDirection.y) * pl.grappleModel.grappleJumpEndSpeed;
    }
}

public class PlayerHurtState : PlayerState
{
    public override void EnterState(PlayerController pl)
    {
        pl.statevisualizer = PlayerController.state.hurt;
        pl.playerAnim.SetTrigger("isHurt");
    }
    public override void Update(PlayerController pl)
    {
        int currentLayer = 0;
        switch (pl.weaponModel.currentWeapon.type)
        {
            case Weapons.WeaponType.BareHand:
                currentLayer = 1;
                break;
            case Weapons.WeaponType.Sword:
                currentLayer = 2;
                break;
            case Weapons.WeaponType.Colossal:
                currentLayer = 3;
                break;
        }
        if (pl.playerAnim.GetCurrentAnimatorStateInfo(currentLayer).normalizedTime > .95f)
        { 
            if(pl.jumpModel.isGrounded)
                pl.ChangeState(pl.moveState); 
            else
                pl.ChangeState(pl.jumpState);
        }
        if(!pl.playerAnim.GetCurrentAnimatorStateInfo(currentLayer).IsName("hurt"))
        {
            if (pl.jumpModel.isGrounded)
                pl.ChangeState(pl.moveState);
            else
                pl.ChangeState(pl.jumpState);
        }
    }
    public override void FixedUpdate(PlayerController pl)
    {

    }
    public override void ExitState(PlayerController pl)
    {
        if(pl.weaponModel.nextWeapon != null)
        {
            pl.GetComponent<AnimationSupporter>().switchWeapon();
        }
    }
}