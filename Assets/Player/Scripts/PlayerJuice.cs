using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJuice : MonoBehaviour
{
    private static PlayerJuice instance;
    public static PlayerJuice Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PlayerJuice>();
            return instance;
        }
    }

    public void HitPause(int duration)
    {
        StartCoroutine(Pause(duration));
    }

    IEnumerator Pause(int duration)
    {
        float pauseTime = duration / 60f;
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(pauseTime);
        Time.timeScale = 1;
    }

}
