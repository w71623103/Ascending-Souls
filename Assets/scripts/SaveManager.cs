using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public Vector3 savePointPos;
    public string saveSceneName;

    public void updateSavePoint(Vector3 newPos, string newScene)
    {
        savePointPos = newPos;
        saveSceneName = newScene;
    }
}
