using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColOut : AttackColBase
{
    protected override int getFinalDmg()
    {
        return (int) (pl.weaponModel.currentWeapon.DamageTable[4] * plAmmo.ammoDamageModifier_HeavyAttack);
    }

    protected override float getHitRecover()
    {
        return pl.weaponModel.currentWeapon.HitRecoverTable[4];
    }

    protected override float getHitBackSpeed()
    {
        return pl.weaponModel.currentWeapon.HitBackSpeedTable[4];
    }

    protected override int getHitPauseTime()
    {
        return pl.weaponModel.currentWeapon.HitPauseTimeTable[4];
    }

    protected override float getVerticalDir()
    {
        return 0f;
    }
}
