using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColUp : AttackColBase
{
    protected override int getFinalDmg()
    {
        return (int) (pl.weaponModel.currentWeapon.DamageTable[5] * plAmmo.ammoDamageModifier_NormalAttack);
    }

    protected override float getHitRecover()
    {
        return pl.weaponModel.currentWeapon.HitRecoverTable[5];
    }

    protected override float getHitBackSpeed()
    {
        return pl.weaponModel.currentWeapon.HitBackSpeedTable[5];
    }

    protected override int getHitPauseTime()
    {
        return pl.weaponModel.currentWeapon.HitPauseTimeTable[5];
    }

    protected override float getVerticalDir()
    {
        return 1f;
    }
}
