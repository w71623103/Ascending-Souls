using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootManAttackCol : EnemyAttackCol
{
    [SerializeField] private FootMan ft;
    // Start is called before the first frame update
    void Start()
    {
        ft = transform.parent.GetComponent<FootMan>();
    }

    // Update is called once per frame
    void Update()
    {
        if(attackVerticalDir == 0f)
            attackDir = new Vector2((int)ft.moveModel.Direction, attackVerticalDir);
        else
            attackDir = new Vector2(0, attackVerticalDir);
    }
}
