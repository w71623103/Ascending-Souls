using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport_touch : MonoBehaviour
{
    [SerializeField] private Transform teleportPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.gameObject.transform.position = teleportPos.position;
        }
    }
}
