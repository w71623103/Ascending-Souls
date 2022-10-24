using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private GameObject targetCamera;
    [SerializeField] private GameObject cameraManager;
    private GameObject currentCamera;
    // Start is called before the first frame update
    void Start()
    {
        currentCamera = cameraManager.GetComponent<CameraManager>().currentCamera;
    }

    /*// Update is called once per frame
    void Update()
    {
        
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(currentCamera != targetCamera)
            {
                targetCamera.SetActive(true);
                currentCamera.SetActive(false);
                cameraManager.GetComponent<CameraManager>().currentCamera = targetCamera;
            }
            
        }
    }
}
