using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHp : Hp
{
    [SerializeField] private float percent;
    [SerializeField] private List<GameObject> dropWeaponList = new List<GameObject>();
    [SerializeField] private List<float> dropWeaponKey = new List<float>();
    [SerializeField] private GameObject dropOBJ = null;
    [SerializeField] private GameObject HpBar;
    // Update is called once per frame
    void Update()
    {
        if(hp == 0)
        {
            Die();
        }
        if(dropWeaponList.Count != dropWeaponKey.Count)
        {
            if (dropWeaponList.Count > dropWeaponKey.Count)
                dropWeaponList.RemoveAt(dropWeaponList.Count - 1);
            else
                dropWeaponKey.RemoveAt(dropWeaponKey.Count - 1);
        }
        percent = hp / mHp;
        HpBar.transform.transform.localScale = new Vector3(percent, 1, 1);
        if (dropWeaponKey.Count > 0 && dropWeaponList.Count > 0)
        {
            if (percent < dropWeaponKey[dropWeaponKey.Count - 1])
            {
                Instantiate(dropWeaponList[dropWeaponList.Count - 1], transform.position, Quaternion.identity);
                dropWeaponList.RemoveAt(dropWeaponList.Count - 1);
                dropWeaponKey.RemoveAt(dropWeaponKey.Count - 1);
            }
        }

    }

    protected override void Die()
    {
        if(dropOBJ != null)
            Instantiate(dropOBJ, transform.position, Quaternion.identity);
        //hp = mHp;
        if (GetComponent<Animator>() != null)
            GetComponent<Animator>().SetBool("isDead", true);
    }

    private void endDie()
    {
        gameObject.SetActive(false);
    }
}
