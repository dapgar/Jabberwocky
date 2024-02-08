using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List <Camera> cameras;

    public int numPlayers = 4;
    private int[] moveData;

    public List<StoneScript> players;
    public int diceRoll;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        moveData = new int[numPlayers];
    }

    private void Start()
    {
        if (!instance)
        {
            instance = this;
        }
    }



    public void TurnOffCamerasBut(Camera camToKeep)
    {
        foreach (Camera c in cameras) 
        {
            c.enabled = false;
        }

        camToKeep.enabled = true;

    }
    
    public void MoveData(int[] moveData) {
        this.moveData = moveData;
    }
}
