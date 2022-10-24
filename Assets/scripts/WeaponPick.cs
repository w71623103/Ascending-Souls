using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPick : Interactable
{
    [SerializeField] Weapons myWeapon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnInteract(PlayerController pl)
    {
        pl.pickWeapon(myWeapon);
        Destroy(gameObject);
    }
}
