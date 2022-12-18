using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHeal : Count
{
    public float healPower = 20f;
    [SerializeField] private PlayerHp playerHp;
    [SerializeField] private MoreMountains.Feedbacks.MMFeedbacks HealFeedback;

    [SerializeField] private float healCD;
    [SerializeField] private float healCDTimer;

    [SerializeField] private TMP_Text healnum;

    protected new virtual void Start()
    {
        base.Start();
        playerHp = GetComponent<PlayerHp>();
    }

    private void Update()
    {
        if(healCDTimer > 0) healCDTimer -= Time.deltaTime;
        if(healnum != null) healnum.text = num.ToString();
    }

    public void Heal()
    {
        if(num > 0 && playerHp.hp != playerHp.mHp && healCDTimer <= 0f)
        {
            playerHp.restoreHP(healPower);

            if(HealFeedback != null)
            {
                HealFeedback?.PlayFeedbacks();
            }

            healCDTimer = healCD;
            decrease(1);
        }
    }
}
