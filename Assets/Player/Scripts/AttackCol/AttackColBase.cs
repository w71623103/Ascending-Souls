using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackColBase: MonoBehaviour
{
    [SerializeField] protected PlayerController pl;
    [SerializeField] protected PlayerAmmo plAmmo;
    [SerializeField] private Vector2 attackDir = Vector2.zero;
    [SerializeField] private float attackVerticalDir = 0f;
    [SerializeField] private Weapons.PushType pushType;
    [SerializeField] protected float hitBackSpeed;

    [SerializeField] protected int finalDamage;
    [SerializeField] protected float hitRecover;

    [Header("Juice")]
    [SerializeField] protected int hitPauseTime;
    // Start is called before the first frame update
    protected void Start()
    {
        if(pl == null) pl = transform.parent.parent.GetComponent<PlayerController>();
        plAmmo = pl.ammo;
    }

    // Update is called once per frame
    protected void Update()
    {
        attackVerticalDir = getVerticalDir();
        if (attackVerticalDir == 0f)
            attackDir = new Vector2((int)pl.moveModel.Direction, attackVerticalDir);
        else
            attackDir = new Vector2(0, attackVerticalDir);

        finalDamage = getFinalDmg();
        hitRecover = getHitRecover();
        hitBackSpeed = getHitBackSpeed();
        hitPauseTime = getHitPauseTime();
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        plAmmo.addAmmo();
        collision.gameObject.GetComponent<Enemy>().OnHit(finalDamage, attackDir, hitBackSpeed, pushType, hitRecover);
        if(collision.gameObject.GetComponent<Hp>() != null)
        {
            //take damage
            collision.gameObject.GetComponent<Hp>().decreaseHP(finalDamage);
        }
        PlayerJuice.Instance.HitPause(hitPauseTime);
    }

    protected abstract int getFinalDmg();

    protected abstract float getHitRecover();

    protected abstract int getHitPauseTime();

    protected abstract float getHitBackSpeed();

    protected abstract float getVerticalDir();
}
