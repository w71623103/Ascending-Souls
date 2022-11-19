using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door_Interactable : Interactable
{
    [SerializeField] private string sceneName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnInteract(PlayerController pl)
    {
        if (SceneManager.GetSceneByName(sceneName) != null)
            SceneManager.LoadScene(sceneName);
        else
            Debug.Log("Scene is null");
    }
}
