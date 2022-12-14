using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMovement : MonoBehaviour
{
    [SerializeField] private Vector3 targetPos;
    [SerializeField] private float reachTolerance = 0.2f;
    [SerializeField] private bool activated = false;
    //[SerializeField] private float speed = 2f;
    [SerializeField] private float timer = 0f;
    [SerializeField] private float moveTime = 2f; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, targetPos) < reachTolerance)
        {
            activated = false;
            timer = 0f;
        }

        if(activated)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, targetPos,timer/moveTime);
        }
    }

    public void activate(Vector3 target)
    {
        activated = true;
        targetPos = target;
    }
}
