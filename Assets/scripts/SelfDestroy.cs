using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField] private float timer;
    [SerializeField] private float timerMax;
    // Start is called before the first frame update
    void Awake()
    {
        timer = timerMax;
    }

    private void Start()
    {
        timer = timerMax;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0f)
            Destroy(gameObject);
    }
}
