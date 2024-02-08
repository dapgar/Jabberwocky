using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int numPlayers = 4;
    public int[] moveData;
    public int[] routeData;

    public int diceRoll;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        moveData = new int[numPlayers];
        routeData = new int[numPlayers];
    }

    private void Start()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    
    public void MoveData(int[] moveData) 
    {
        this.moveData = moveData;
        Debug.Log("MoveData Test: " + moveData[0]);
    }
}
