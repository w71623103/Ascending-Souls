using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using MoreMountains.Feedbacks;

public class SavePoint : Interactable
{
    [SerializeField] private Vector3 savePos;
    [SerializeField] private string sceneName;
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private MMFeedbacks SaveFeedback;

    // Start is called before the first frame update
    void Start()
    {
        saveManager = GameObject.Find("SaveManager").GetComponent<SaveManager>();
        savePos = transform.position;
        sceneName = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnInteract(PlayerController pl)
    {
        if(SaveFeedback != null)SaveFeedback?.PlayFeedbacks(transform.position);
        saveManager.updateSavePoint(savePos, sceneName);
        pl.GetComponent<Hp>().hp = pl.GetComponent<Hp>().mHp;
        pl.GetComponent<PlayerHeal>().num = pl.GetComponent<PlayerHeal>().numMax;
        pl.GetComponent<PlayerAmmo>().switchPoint = pl.GetComponent<PlayerAmmo>().switchPointMax;
    }
}
