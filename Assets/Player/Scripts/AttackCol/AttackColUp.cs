using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColUp : AttackColBase
{
    protected override int getFinalDmg()
    {
        return (int) (pl.weaponModel.currentWeapon.DamageTable[pl.weaponModel.currentWeapon.DamageTable.Count - 1] * plAmmo.ammoDamageModifier_NormalAttack);
    }

    protected override float getHitRecover()
    {
        return pl.weaponModel.currentWeapon.HitRecoverTable[pl.weaponModel.currentWeapon.DamageTable.Count - 1];
    }

    protected override float getHitBackSpeed()
    {
        return pl.weaponModel.currentWeapon.HitBackSpeedTable[pl.weaponModel.currentWeapon.DamageTable.Count - 1];
    }

    protected override int getHitPauseTime()
    {
        return pl.weaponModel.currentWeapon.HitPauseTimeTable[pl.weaponModel.currentWeapon.DamageTable.Count - 1];
    }

    protected override float getVerticalDir()
    {
        return 1f;
    }
}
