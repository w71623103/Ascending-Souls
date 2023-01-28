using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootManHp : Hp
{
    [SerializeField] private float percent;
    [SerializeField] private GameObject dropOBJ = null;
    [SerializeField] private GameObject HpBar;
    // Update is called once per frame
    void Update()
    {
        if(hp == 0)
        {
            Die();
        }
        percent = hp / mHp;
        HpBar.transform.transform.localScale = new Vector3(percent, 1, 1);
    }

    protected override void Die()
    {
        if(dropOBJ != null)
            Instantiate(dropOBJ, transform.position, Quaternion.identity);
        if (GetComponent<Animator>() != null)
            GetComponent<Animator>().SetBool("isDead", true);
    }

    private void endDie()
    {
        gameObject.SetActive(false);
    }
}
