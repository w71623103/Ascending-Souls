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
    [SerializeField] private int damage;
    [Header("Juice")]
    [SerializeField] private int hitPauseTime;

    // Start is called before the first frame update
    void Start()
    {
        pl = transform.parent.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (attackVerticalDir == 0f)
            attackDir = new Vector2((int)pl.moveModel.Direction, attackVerticalDir);
        else
            attackDir = new Vector2(0, attackVerticalDir);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<Enemy>().OnHit(attackDir, hitBackSpeed, damage, pushType);
        PlayerJuice.Instance.HitPause(hitPauseTime);
    }


}
