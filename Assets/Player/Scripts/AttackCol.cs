using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCol : MonoBehaviour
{
    [SerializeField] private PlayerController pl;
    [SerializeField] private PlayerAmmo plAmmo;
    [SerializeField] private Vector2 attackDir = Vector2.zero;
    [SerializeField] private float attackVerticalDir = 0f;
    [SerializeField] private Weapons.PushType pushType;
    [SerializeField] private float hitBackSpeed;
    [SerializeField] private bool isNormalAttack = true;
    [SerializeField] private int damage;
    [SerializeField] private int finalDamage;
    [SerializeField] private float hitRecover;
    [Header("Juice")]
    [SerializeField] private int hitPauseTime;
    // Start is called before the first frame update
    void Start()
    {
        pl = transform.parent.GetComponent<PlayerController>();
        plAmmo = pl.ammo;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackVerticalDir == 0f)
            attackDir = new Vector2((int)pl.moveModel.Direction, attackVerticalDir);
        else
            attackDir = new Vector2(0, attackVerticalDir);
        if (isNormalAttack) finalDamage = (int) (damage * plAmmo.ammoDamageModifier_NormalAttack);
        else finalDamage = (int)(damage * plAmmo.ammoDamageModifier_HeavyAttack);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        plAmmo.consumeAmmo();
        collision.gameObject.GetComponent<Enemy>().OnHit(attackDir, hitBackSpeed, pushType, hitRecover);
        if(collision.gameObject.GetComponent<Hp>() != null)
        {
            //take damage
            collision.gameObject.GetComponent<Hp>().decreaseHP(finalDamage);
        }
        PlayerJuice.Instance.HitPause(hitPauseTime);
    }


}
