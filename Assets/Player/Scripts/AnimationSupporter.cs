using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSupporter : MonoBehaviour
{
    [SerializeField] PlayerController pl;

    private void Awake()
    {
        pl = GetComponent<PlayerController>();
    }

    public void moveForward(float velocity)
    {
        float vel = (int)pl.moveModel.Direction * velocity;
        pl.playerRB.velocity = new Vector2 (vel, pl.playerRB.velocity.y);
    }

    public void EndDash()
    {
        if (pl.generalState == pl.dashState)
        {
            if (pl.jumpModel.isGrounded) pl.ChangeState(pl.moveState);
            else pl.ChangeState(pl.jumpState);
        }
    }

    public void EndHurt()
    {
        if (pl.generalState == pl.hurtState) pl.ChangeState(pl.moveState);
    }

    public void allowInput()
    {
        pl.attackModel.allowInput = true;
        pl.attackModel.comboed = false;
    }

    public void allowDash()
    {
        pl.dashModel.allowDash = true;
    }

    public void endAttack()
    {
        pl.attackModel.allowInput = false;
        if (pl.generalState == pl.attackStateSP)
        {
            pl.ChangeState(pl.moveState);
        }
        else
        {
            if (!pl.attackModel.comboed)
            {
                //if(!pl.jumpModel.isGrounded) pl.attackModel.airAttackCount--;
                if (pl.jumpModel.isGrounded) pl.ChangeState(pl.moveState);
                else pl.ChangeState(pl.jumpState);
            }
            else if (pl.playerAnim.GetInteger("AttackLight") != pl.attackModel.attackCount)
            {
                //play next combo
                pl.playerAnim.SetInteger("AttackLight", pl.attackModel.attackCount);
            }
        }
    }

    public void switchWeapon()
    {
        GameObject weaponDrop;
        //switch layer
        if (pl.weaponModel.nextWeapon != null)
        {
            switch (pl.weaponModel.currentWeapon.type)
            {
                case Weapons.WeaponType.Sword:
                    weaponDrop = Instantiate(pl.weaponModel.swordPick, transform.position, Quaternion.identity);
                    weaponDrop.GetComponent<WeaponPick>().ammoInThis = pl.weaponModel.ammo;
                    break;
                case Weapons.WeaponType.GreatSword:
                    weaponDrop = Instantiate(pl.weaponModel.greatSwordPick, transform.position, Quaternion.identity);
                    weaponDrop.GetComponent<WeaponPick>().ammoInThis = pl.weaponModel.ammo;
                    break;
            }
            switch (pl.weaponModel.nextWeapon.type)
            {
                case Weapons.WeaponType.BareHand:
                    pl.playerAnim.SetLayerWeight(pl.playerAnim.GetLayerIndex("BareHand"), 1f);
                    pl.playerAnim.SetLayerWeight(pl.playerAnim.GetLayerIndex("Sword"), 0f);
                    pl.playerAnim.SetLayerWeight(pl.playerAnim.GetLayerIndex("GreatSword"), 0f);
                    break;
                case Weapons.WeaponType.Sword:
                    pl.playerAnim.SetLayerWeight(pl.playerAnim.GetLayerIndex("BareHand"), 0f);
                    pl.playerAnim.SetLayerWeight(pl.playerAnim.GetLayerIndex("Sword"), 1f);
                    pl.playerAnim.SetLayerWeight(pl.playerAnim.GetLayerIndex("GreatSword"), 0f);
                    break;
                case Weapons.WeaponType.GreatSword:
                    pl.playerAnim.SetLayerWeight(pl.playerAnim.GetLayerIndex("BareHand"), 0f);
                    pl.playerAnim.SetLayerWeight(pl.playerAnim.GetLayerIndex("Sword"), 0f);
                    pl.playerAnim.SetLayerWeight(pl.playerAnim.GetLayerIndex("GreatSword"), 1f);
                    break;
            }
            //set trigger
            pl.playerAnim.SetTrigger("WeaponIn");
            pl.weaponModel.currentWeapon = pl.weaponModel.nextWeapon;
            if (pl.weaponModel.currentWeapon.type == Weapons.WeaponType.BareHand) pl.weaponModel.ammo = pl.weaponModel.ammoMax;
            pl.weaponModel.nextWeapon = null;
            
        }
        /*else
        {
            endAttack();
        }*/
    }

    public void setGravity(float scale)
    {
        pl.playerRB.gravityScale = scale;
    }

    public void reduceAirAttack()
    {
        pl.attackModel.airAttackCount--;
    }

    public void smallJumpInAir()
    {
        pl.playerRB.velocity = new Vector2(pl.playerRB.velocity.x, pl.jumpModel.smallJumpSpeed);
    }

    public void allowTurn()
    {
        pl.attackModel.allowTurn = true;
        if (pl.moveModel.HorizontalMovement < 0f)
            pl.moveModel.Direction = PlayerMoveModel.PlayerDirection.Left;
        else if (pl.moveModel.HorizontalMovement > 0f)
            pl.moveModel.Direction = PlayerMoveModel.PlayerDirection.Right;
    }

    public void disableTurn()
    {
        pl.attackModel.allowTurn = false;
    }

    public void clearVerticalVel()
    {
        pl.playerRB.velocity = new Vector2(pl.playerRB.velocity.x, 0f);
    }
}
