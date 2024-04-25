using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public int numPlayers;
    [HideInInspector] public int[] moveData;
    [HideInInspector] public int[] routeData;
    [HideInInspector] public Vector3[] playersPos;
    [HideInInspector] public Quaternion[] playerRots;

    private int weightBase = 2;
    private int weightIncrease = 2;
    private int[] miniGameWeights = new int[4]; //MAKE THIS EQUAL TO THE MINILOAD MANAGER GAME NUMBER


    // Win con
    public List<int> playerRankings;

    [HideInInspector] public int diceRoll;

    public bool playersMoving;

    [HideInInspector] public int devMinigameNumber = -1;

    private void Awake() {
        DontDestroyOnLoad(transform.gameObject);
        
        playersMoving = false;

        for (int i = 0; i < miniGameWeights.Length; i++)
        {
            miniGameWeights[i] = weightBase;
        }
        // Must be called to initialize (for now), but in future we will need to have gamemanger be on mainmenu scene so it's valid early on
        SetNumPlayers(numPlayers);
    }

    public void SetNumPlayers(int num) {
        numPlayers = num;
        Debug.Log("PLAYERS: " + num);
        moveData = new int[numPlayers];
        routeData = new int[numPlayers];
        playersPos = new Vector3[numPlayers];
        playerRots = new Quaternion[numPlayers];
    }

    private void Start() {
        if (!instance) {
            instance = this;
        }
        else {
            Destroy(this);
        }
    }

    public void MoveData(int[] moveData) {
        this.moveData = moveData;
    }

    public int RandomGame()
    {
        int gameToReturn = 0;

        int randomWeight = Random.Range(0, miniGameWeights.Sum());
        for (int i = 0; i < miniGameWeights.Length; i++)
        {
            randomWeight -= miniGameWeights[i];
            if (randomWeight < 0)
            {
                gameToReturn = i;
                break;
            }
        }

        for (int i = 0; i < miniGameWeights.Length; i++)
        {
            if (i == gameToReturn)
            {
                //weights[i] += weightDecrease;
                miniGameWeights[i] = 0;
            }
            else
            {
                if (miniGameWeights[i] < weightBase)
                {
                    miniGameWeights[i] += 1;
                }
                else
                {
                    miniGameWeights[i] += weightIncrease;
                }
            }
        }

        return gameToReturn;
    }

    public void ResetGame()
    {
        SetNumPlayers(numPlayers);
    }
}
