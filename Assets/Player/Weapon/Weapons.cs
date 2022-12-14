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
    public float ammoConsume;
    public Sprite icon;
    public Sprite lowAmmoIcon;

    [Tooltip("0-2: Normal Attack Damage; 3: Attack In Damage; 4£ºAttack Out Damage; 5: Attack Up Damage")]
    public List<int> DamageTable = new List<int>();

    [Tooltip("0-2: Normal Attack HitRecover; 3: Attack In HitRecover; 4£ºAttack Out HitRecover; 5: Attack Up HitRecover")]
    public List<float> HitRecoverTable = new List<float>();

    [Tooltip("0-2: Normal Attack HitBackSpeed; 3: Attack In HitBackSpeed; 4£ºAttack Out HitBackSpeed; 5: Attack Up HitBackSpeed")]
    public List<float> HitBackSpeedTable = new List<float>();

    [Tooltip("0-2: Normal Attack HitPauseTime; 3: Attack In HitPauseTime; 4£ºAttack Out HitPauseTime; 5: Attack Up HitPauseTime")]
    public List<int> HitPauseTimeTable = new List<int>();
}
