using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPick : Interactable
{
    [SerializeField] Weapons myWeapon;
    public float ammoInThis;
    // Start is called before the first frame update
    void Start()
    {
        
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
        pl.weaponModel.ammo = ammoInThis;
        pl.pickWeapon(myWeapon);
        Destroy(gameObject);
    }
}
