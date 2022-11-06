using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hp : MonoBehaviour
{
    public float hp;
    public float mHp = 100f;
    public enum DamageMode
    {
        single,
        twice,
        dot,
    }

    protected void OnEnable()
    {
        hp = mHp;
    }

    // Start is called before the first frame update
    protected void Start()
    {
        hp = mHp;
    }

    public void decreaseHP(float damage)
    {
        if(hp - damage >= 0)
            hp -= damage;
        else
            hp = 0;
    }

    protected abstract void Die();
}
