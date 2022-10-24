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
    }

    public enum PushType
    {
        inPlace,
        pushBack,
        upKa,
        quickFall,
    }

    public WeaponType type;
}
