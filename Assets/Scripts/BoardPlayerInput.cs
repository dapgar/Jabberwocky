using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPlayerInput : MonoBehaviour
{
    DiceManager diceManager;
    // Start is called before the first frame update
    void Awake()
    {
        diceManager = GameObject.Find("_DiceManager").GetComponent<DiceManager>();
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
            diceManager.TryStartTurn();
        }
    }
}
