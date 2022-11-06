using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManAttackCol : EnemyAttackCol
{
    [SerializeField] private SwordMan sw;
    // Start is called before the first frame update
    void Start()
    {
        sw = transform.parent.GetComponent<SwordMan>();
    }

    // Update is called once per frame
    void Update()
    {
        if(attackVerticalDir == 0f)
            attackDir = new Vector2((int)sw.moveModel.Direction, attackVerticalDir);
        else
            attackDir = new Vector2(0, attackVerticalDir);
    }
}
