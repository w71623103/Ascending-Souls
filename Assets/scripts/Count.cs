using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Count : MonoBehaviour
{
    public int num;
    public int numMax;

    protected void OnEnable()
    {
        num = numMax;
    }

    protected void Start()
    {
        num = numMax;
    }

    public void decrease(int amount)
    {
        if(num - amount >= 0)
            num -= amount;
        else
            num = 0;
    }

    public void increase(int amount)
    {
        if (num + amount <= numMax)
            num += amount;
        else
            num = numMax;
    }
}
