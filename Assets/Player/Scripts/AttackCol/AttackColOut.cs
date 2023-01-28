using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColOut : AttackColBase
{
    protected override int getFinalDmg()
    {
        return (int) (pl.weaponModel.currentWeapon.DamageTable[pl.weaponModel.currentWeapon.DamageTable.Count - 2] * plAmmo.ammoDamageModifier_HeavyAttack);
    }

    protected override float getHitRecover()
    {
        return pl.weaponModel.currentWeapon.HitRecoverTable[pl.weaponModel.currentWeapon.DamageTable.Count - 2];
    }

    protected override float getHitBackSpeed()
    {
        return pl.weaponModel.currentWeapon.HitBackSpeedTable[pl.weaponModel.currentWeapon.DamageTable.Count - 2];
    }

    protected override int getHitPauseTime()
    {
        return pl.weaponModel.currentWeapon.HitPauseTimeTable[pl.weaponModel.currentWeapon.DamageTable.Count - 2];
    }

    protected override float getVerticalDir()
    {
        return 0f;
    }
}
