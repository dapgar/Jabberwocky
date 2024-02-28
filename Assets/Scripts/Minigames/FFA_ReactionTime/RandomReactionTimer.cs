using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomReactionTimer : MonoBehaviour
{
    [SerializeField] private string nameInEditor;

    [SerializeField] private float minRange;
    [SerializeField] private float maxRange;
    public float time;

    public delegate void OnTimerEnd();
    public event OnTimerEnd onTimerEnd;

    public void Start()
    {
        Reset();
    }

    public void Reset()
    {
        time = Random.Range(minRange, maxRange);
    }

    public void Countdown()
    {
        time -= Time.deltaTime;

        if (TimerDone())
        {
            onTimerEnd?.Invoke();
        }
    }

    private bool TimerDone()
    {
        return (time <= 0);
    }
}
