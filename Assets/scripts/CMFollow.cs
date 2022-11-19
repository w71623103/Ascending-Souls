using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CMFollow : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera v_Cam;
    // Start is called before the first frame update
    void Start()
    {
        v_Cam = GetComponent<CinemachineVirtualCamera>();
        v_Cam.Follow = GameObject.FindWithTag("PlayerCameraPos").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
