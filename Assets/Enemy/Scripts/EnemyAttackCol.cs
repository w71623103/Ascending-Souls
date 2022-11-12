using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackCol : MonoBehaviour
{
    [SerializeField] protected Vector2 attackDir = Vector2.zero;
    [SerializeField] protected float attackVerticalDir = 0f;
    [SerializeField] protected Weapons.PushType pushType;
    [SerializeField] protected float hitBackSpeed;
    [SerializeField] protected int damage;
    [Header("Juice")]
    [SerializeField] protected int hitPauseTime;

    // Start is called before the first frame update
    void Start()
    {
        /*em = transform.parent.GetComponent<Enemy>();*/
    }

    // Update is called once per frame
    void Update()
    {
        /*if (attackVerticalDir == 0f)
            attackDir = new Vector2((int)em.moveModel.Direction, attackVerticalDir);
        else
            attackDir = new Vector2(0, attackVerticalDir);*/
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<PlayerController>().OnHit(attackDir, hitBackSpeed, pushType);
        if(collision.gameObject.GetComponent<Hp>() != null)
        {
            //take damage
            collision.gameObject.GetComponent<Hp>().decreaseHP(damage);
        }
        //PlayerJuice.Instance.HitPause(hitPauseTime);
    }


}
