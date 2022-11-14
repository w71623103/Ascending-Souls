using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitRecoverTolerance : Stat
{
    [SerializeField] private float percent;
    [SerializeField] private GameObject hitRBar;

    protected new void Start()
    {
        num = 0f;
    }

    protected new void OnEnable()
    {
        num = 0f;
    }

    void Update()
    {
        percent = num / numMax;
        hitRBar.transform.localScale = new Vector3(1, percent, 1);
    }
}
