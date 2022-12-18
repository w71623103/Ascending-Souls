using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHp : Hp
{
    [SerializeField] private float percent;
    [SerializeField] private GameObject HpBar;
    [SerializeField] private PlayerHeal heal;
    [SerializeField] private SaveManager saveManager;
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
        Time.timeScale = 1;
        GetComponent<PlayerController>().grappleModel.GrappleSensor.GetComponent<GrappleArea>().clearPossiblePoints();
        hp = mHp;
        gameObject.transform.position = saveManager.savePointPos;
        heal.increase(heal.numMax);
        SceneManager.LoadScene("Continue");
    }
}
