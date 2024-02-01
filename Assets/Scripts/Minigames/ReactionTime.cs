using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionTime : MonoBehaviour
{
    float currentTimer;
    float timeGoal;

    [SerializeField] private float minTime;
    [SerializeField] private float maxTime;

    

    // Start is called before the first frame update
    void Start()
    {
        timeGoal = Random.Range(minTime, maxTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
