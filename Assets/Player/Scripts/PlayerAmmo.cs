using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAmmo : Stat
{
    [SerializeField] public float percent;
    [SerializeField] private GameObject ammoBar;
    [SerializeField] private GameObject[] weaponIconFrame;
    [SerializeField] private PlayerController pl;

    [Header("Switch Weapon Point")]
    public int switchPoint = 0;
    public int switchPointMax = 2;
    public TMPro.TMP_Text switchPointText;

    public float ammoDamageModifier_NormalAttack;
    public float ammoDamageModifier_HeavyAttack;
    [SerializeField] protected MoreMountains.Feedbacks.MMFeedbacks ammoFeedback;

    // Update is called once per frame
    void Update()
    {
        percent = num / numMax;
        ammoBar.transform.transform.localScale = new Vector3(percent, 1, 1);
        if (switchPoint == 0) ammoDamageModifier_NormalAttack = (1f + percent);
        else ammoDamageModifier_NormalAttack = 2f;
        ammoDamageModifier_HeavyAttack = 8;
        
        switch(pl.weaponModel.currentWeapon.type)
        {
            case Weapons.WeaponType.BareHand:
                weaponIconFrame[0].SetActive(true);
                weaponIconFrame[1].SetActive(false);
                weaponIconFrame[2].SetActive(false);
                weaponIconFrame[3].SetActive(false);
                break;
            case Weapons.WeaponType.Sword:
                weaponIconFrame[0].SetActive(false);
                weaponIconFrame[1].SetActive(true);
                weaponIconFrame[2].SetActive(false);
                weaponIconFrame[3].SetActive(false);
                break;
            case Weapons.WeaponType.DualBlade:
                weaponIconFrame[0].SetActive(false);
                weaponIconFrame[1].SetActive(false);
                weaponIconFrame[2].SetActive(true);
                weaponIconFrame[3].SetActive(false);
                break;
            case Weapons.WeaponType.GreatSword:
                weaponIconFrame[0].SetActive(false);
                weaponIconFrame[1].SetActive(false);
                weaponIconFrame[2].SetActive(false);
                weaponIconFrame[3].SetActive(true);
                break;
        }

        if(percent >= 1 && switchPoint < switchPointMax)
        {
            switchPoint++;
            num = 0f;
        }

        if(switchPointText != null)
        {
            switchPointText.text = switchPoint.ToString();
        }
    }

    public void addAmmo()
    {
        increase(pl.weaponModel.currentWeapon.ammoConsume);
        ammoFeedback?.PlayFeedbacks();
    }

    public void useSwitchPoint()
    {
        if(switchPoint - 1 > 0)
        {
            switchPoint--;
        }
        else
        {
            switchPoint = 0;
        }
    }

}
