using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private string nameInEditor;

    [SerializeField] private float maxTime = 3;
    public float time;

    public delegate void OnTimerEnd();
    public event OnTimerEnd onTimerEnd;

    public void Start()
    {
        Reset();
    }

    public void Reset()
    {
        time = maxTime;
    }

    public void Countdown()
    {
        time -= Time.deltaTime;

        if (TimerDone())
        {
            Debug.Log("Timer Done!");
            onTimerEnd?.Invoke();
        }
    }

    private bool TimerDone()
    {
        return (time <= 0);
    }
}
