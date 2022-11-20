using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponPick : Interactable
{
    [SerializeField] Weapons myWeapon;
    [SerializeField] GameObject ammoBar;
    public float ammoInThis;
    // Start is called before the first frame update
    void Start()
    {
        ammoBar.transform.localScale = new Vector3(1f, ammoInThis / 100, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if(ammoInThis <= 0)
        {
            Destroy(gameObject);
        }
    }

    public override void OnInteract(PlayerController pl)
    {
        pl.ammo.ammoNext = ammoInThis;
        pl.pickWeapon(myWeapon);
        Destroy(gameObject);
    }
}
