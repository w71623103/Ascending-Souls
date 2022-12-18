using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private GameObject targetCamera;
    [SerializeField] private CameraManager cm;
    //private GameObject currentCamera;

/*    [SerializeField] private bool playerIn;*/
    // Start is called before the first frame update
    void Start()
    {
        //currentCamera = cameraManager.GetComponent<CameraManager>().currentCamera;
    }

    /*// Update is called once per frame
    void Update()
    {
        
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            /*playerIn = true;*/
            if(cm.currentCamera != targetCamera)
            {
                targetCamera.SetActive(true);
                cm.currentCamera.SetActive(false);
                cm.currentCamera = targetCamera;
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            /*playerIn = false;*/
        }
    }
}
