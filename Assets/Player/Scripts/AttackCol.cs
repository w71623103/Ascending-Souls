using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCol : MonoBehaviour
{
    [SerializeField] private PlayerController pl;
    [SerializeField] private Vector2 attackDir = Vector2.zero;
    [SerializeField] private float attackVerticalDir = 0f;
    [SerializeField] private Weapons.PushType pushType;
    [SerializeField] private float hitBackSpeed;
    [SerializeField] private bool dmgAmmoDecrease = true;
    [SerializeField] private int damage;
    [SerializeField] private int finalDamage;
    private Dictionary<float, float> ammoDamageCurve = new Dictionary<float, float>();
    [SerializeField] private float currentPercent;
    [Header("Juice")]
    [SerializeField] private int hitPauseTime;
    // Start is called before the first frame update
    void Start()
    {
        pl = transform.parent.GetComponent<PlayerController>();
        ammoDamageCurve.Add(0f, 0.2f);
        ammoDamageCurve.Add(0.3f, 0.5f);
        ammoDamageCurve.Add(0.5f, 0.7f);
        ammoDamageCurve.Add(1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (attackVerticalDir == 0f)
            attackDir = new Vector2((int)pl.moveModel.Direction, attackVerticalDir);
        else
            attackDir = new Vector2(0, attackVerticalDir);
        currentPercent = pl.weaponModel.getCurrentPercent();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (dmgAmmoDecrease)
            finalDamage = (int)(damage * ammoDamageCurve[currentPercent]);
        else
            finalDamage = (int) (damage * 1 / ammoDamageCurve[currentPercent]);
        pl.weaponModel.comsueAmmo();
        collision.gameObject.GetComponent<Enemy>().OnHit(attackDir, hitBackSpeed, pushType);
        if(collision.gameObject.GetComponent<Hp>() != null)
        {
            //take damage
            collision.gameObject.GetComponent<Hp>().decreaseHP(finalDamage);
        }
        PlayerJuice.Instance.HitPause(hitPauseTime);
    }


}
