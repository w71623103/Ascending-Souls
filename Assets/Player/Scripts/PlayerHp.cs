using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHp : Hp
{

    // Update is called once per frame
    void Update()
    {
        //update UI Hp Bar
    }

    protected override void Die()
    {
        Debug.Log("I'm dead.");
    }
}
