using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoSwitchSceneContinue : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    [SerializeField] private float timer;
    [SerializeField] private float SwitchAfterSeconds;
    [SerializeField] private SaveManager saveManager;
    // Start is called before the first frame update
    void Start()
    {
        saveManager = GameObject.Find("SaveManager").GetComponent<SaveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > SwitchAfterSeconds)
            SceneManager.LoadScene(saveManager.saveSceneName);
    }
}
