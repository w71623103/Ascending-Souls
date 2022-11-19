using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoSwitchScene : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    [SerializeField] private float timer;
    [SerializeField] private float SwitchAfterSeconds;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > SwitchAfterSeconds)
            SceneManager.LoadScene(nextSceneName);
    }
}
