using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stat : MonoBehaviour
{
    public float num;
    public float numMax = 100f;

    protected void OnEnable()
    {
        num = numMax;
    }

    // Start is called before the first frame update
    protected void Start()
    {
        num = numMax;
    }

    public void decrease(float amount)
    {
        if(num - amount >= 0)
            num -= amount;
        else
            num = 0;
    }
}
