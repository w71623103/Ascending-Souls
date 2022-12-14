using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColIn : AttackColBase
{
    protected override int getFinalDmg()
    {
        return pl.weaponModel.currentWeapon.DamageTable[3] * 5;
    }

    protected override float getHitRecover()
    {
        return pl.weaponModel.currentWeapon.HitRecoverTable[3];
    }

    protected override float getHitBackSpeed()
    {
        return pl.weaponModel.currentWeapon.HitBackSpeedTable[3];
    }

    protected override int getHitPauseTime()
    {
        return pl.weaponModel.currentWeapon.HitPauseTimeTable[3];
    }

    protected override float getVerticalDir()
    {
        return 0f;
    }
}
