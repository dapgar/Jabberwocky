using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPlayerInput : MonoBehaviour
{
    TurnManager turnManager;
    // Start is called before the first frame update
    void Awake()
    {
        turnManager = GameObject.Find("_TurnManager").GetComponent<TurnManager>();

    }

    private void Start()
    {
        BoardManager.instance.SetupPlayer(this.gameObject);
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
