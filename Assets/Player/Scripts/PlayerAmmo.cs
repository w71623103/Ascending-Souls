using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAmmo : Stat
{
    [SerializeField] private float percent;
    [SerializeField] private GameObject ammoBar;
    [SerializeField] private GameObject dropWeaponButton;
    [SerializeField] private PlayerController pl;
    public float ammoNext = 0f;
    public float ammoDamageModifier_NormalAttack;
    public float ammoDamageModifier_HeavyAttack;
    [SerializeField] protected MoreMountains.Feedbacks.MMFeedbacks ammoFeedback;

    // Update is called once per frame
    void Update()
    {
        percent = num / numMax;
        ammoBar.transform.transform.localScale = new Vector3(percent, 1, 1);
        if(ammoDamage() != 0f)
        {
            ammoDamageModifier_NormalAttack = ammoDamage();
            ammoDamageModifier_HeavyAttack = 1 / ammoDamage();
        }
        if (percent < 0.5f)
        { 
            pl.weaponModel.WeaponIcon.GetComponent<UnityEngine.UI.Image>().sprite = pl.weaponModel.currentWeapon.lowAmmoIcon;
            if(dropWeaponButton != null) dropWeaponButton.SetActive(true);
        }
        else
        { 
            pl.weaponModel.WeaponIcon.GetComponent<UnityEngine.UI.Image>().sprite = pl.weaponModel.currentWeapon.icon;
            if (dropWeaponButton != null) dropWeaponButton.SetActive(false);
        }

    }

    public void consumeAmmo()
    {
        decrease(pl.weaponModel.currentWeapon.ammoConsume);
        ammoFeedback?.PlayFeedbacks();
    }

    public float ammoDamage()
    {
        if (percent > 0.5f && percent <= 1f)
            return 1f;
        else if (percent <= 0.5f && percent > 0.3f)
            return 0.5f;
        else if (percent <= 0.3f && percent > 0f)
            return 0.3f;
        else if (percent <= 0f)
            return 0.1f;

        return 0f;
    }
}
