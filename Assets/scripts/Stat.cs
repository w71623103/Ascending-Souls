using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stat : MonoBehaviour
{
    public float num;
    public float numMax = 100f;
    public float numStart;

    protected void OnEnable()
    {
        num = numStart;
    }

    // Start is called before the first frame update
    protected void Start()
    {
        num = numStart;
    }

    public void decrease(float amount)
    {
        if(num - amount >= 0)
            num -= amount;
        else
            num = 0;
    }

    public void increase(float amount)
    {
        if (num + amount < numMax)
            num += amount;
        else
            num = numMax;
    }
}
