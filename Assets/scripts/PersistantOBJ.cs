using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistantOBJ : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
