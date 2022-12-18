using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColNormal : AttackColBase
{
    protected override int getFinalDmg()
    {
        if(pl.attackModel.attackCount > 0)
            return (int) (pl.weaponModel.currentWeapon.DamageTable[pl.attackModel.attackCount - 1] * plAmmo.ammoDamageModifier_NormalAttack);
        else
            return (int)(pl.weaponModel.currentWeapon.DamageTable[0] * plAmmo.ammoDamageModifier_NormalAttack);
    }

    protected override float getHitRecover()
    {
        if (pl.attackModel.attackCount > 0)
            return pl.weaponModel.currentWeapon.HitRecoverTable[pl.attackModel.attackCount - 1];
        else
            return pl.weaponModel.currentWeapon.HitRecoverTable[0];
    }

    protected override float getHitBackSpeed()
    {
        if (pl.attackModel.attackCount > 0)
            return pl.weaponModel.currentWeapon.HitBackSpeedTable[pl.attackModel.attackCount - 1];
        else
            return pl.weaponModel.currentWeapon.HitBackSpeedTable[0];

    }

    protected override int getHitPauseTime()
    {
        if (pl.attackModel.attackCount > 0)
            return pl.weaponModel.currentWeapon.HitPauseTimeTable[pl.attackModel.attackCount - 1];
        else
            return pl.weaponModel.currentWeapon.HitPauseTimeTable[0];

    }

    protected override float getVerticalDir()
    {
        return 0f;
    }
}
