using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimeManager : MonoBehaviour
{
    public float gameTime;
    public float barSpeed = 0.1f;
    public bool done;
    void Update()
    {
        RunTimer();
    }

    public void RunTimer()
    {
        if (this.enabled)
        {
            StartCoroutine(RunTimerRoutine());
        }
    }
    private IEnumerator RunTimerRoutine()
    {
        while (gameTime > 0)
        {
            yield return new WaitForSecondsRealtime(barSpeed);
            gameTime -= barSpeed;
        }
        done = true;
    }
}
