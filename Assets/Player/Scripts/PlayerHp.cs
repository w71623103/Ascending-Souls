using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHp : Hp
{
    [SerializeField] private float percent;
    [SerializeField] private GameObject HpBar;

    // Update is called once per frame
    void Update()
    {
        //update UI Hp Bar
        if (hp == 0)
        {
            Die();
        }
        percent = hp / mHp;
        HpBar.transform.transform.localScale = new Vector3(percent, 1, 1);
    }

    protected override void Die()
    {
        Debug.Log("I'm dead.");
        hp = mHp;
        //UnityEngine.SceneManagement.SceneManager.LoadScene("TestYourLimit");
    }
}
