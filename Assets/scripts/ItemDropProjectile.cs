using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropProjectile : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] Vector2 dir;
    [SerializeField] float vSpeed = 10f;
    [SerializeField] float hSpeedBoundABS = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        /*rb.gravityScale = 2f;*/
        dir = new Vector2(Random.Range(-1 * hSpeedBoundABS, hSpeedBoundABS), 1);

        rb.velocity = dir * vSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
