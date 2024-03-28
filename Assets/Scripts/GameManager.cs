using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public int numPlayers;
    [HideInInspector] public int[] moveData;
    [HideInInspector] public int[] routeData;
    [HideInInspector] public Vector3[] playersPos;
    [HideInInspector] public Quaternion[] playerRots;

    // Win con
    public List<int> playerRankings;

    [HideInInspector] public int diceRoll;

    [HideInInspector] public bool playersMoving;

    [HideInInspector] public int devMinigameNumber = -1;

    private void Awake() {
        DontDestroyOnLoad(transform.gameObject);
        
        playersMoving = false;
        
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
}
