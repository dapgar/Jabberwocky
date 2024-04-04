using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPlayerInput : MonoBehaviour
{
    DiceManager turnManager;
    // Start is called before the first frame update
    void Awake()
    {
        turnManager = GameObject.Find("_TurnManager").GetComponent<DiceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnButton(bool value)
    {
        // PRESS
        if (value)
        {
            turnManager.TryStartTurn();
        }
    }
}
