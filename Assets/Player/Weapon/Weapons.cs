using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*[System.Serializable]*/
[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapons : ScriptableObject
{
    public enum WeaponType
    {
        BareHand,
        Sword,
        GreatSword,
        Colossal,
        Spear,
        DualBlade,
    }

    public enum PushType
    {
        inPlace,
        pushBack,
        upKa,
        quickFall,
    }

    public WeaponType type;
    public int attackCountMax;
    public float ammoConsume;
    public Sprite icon;
    public Sprite lowAmmoIcon;

    [Tooltip("0-4: Normal Attack Damage; 5: Attack In Damage; 6£ºAttack Out Damage; 7: Attack Up Damage")]
    public List<int> DamageTable = new List<int>();

    [Tooltip("0-4: Normal Attack HitRecover; 5: Attack In HitRecover; 6£ºAttack Out HitRecover; 7: Attack Up HitRecover")]
    public List<float> HitRecoverTable = new List<float>();

    [Tooltip("0-4: Normal Attack HitBackSpeed; 5: Attack In HitBackSpeed; 6£ºAttack Out HitBackSpeed; 7: Attack Up HitBackSpeed")]
    public List<float> HitBackSpeedTable = new List<float>();

    [Tooltip("0-4: Normal Attack HitPauseTime; 5: Attack In HitPauseTime; 6£ºAttack Out HitPauseTime; 7: Attack Up HitPauseTime")]
    public List<int> HitPauseTimeTable = new List<int>();
}
