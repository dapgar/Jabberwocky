using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public int numPlayers;
    [HideInInspector] public int[] moveData;
    [HideInInspector] public int[] routeData;
    [HideInInspector] public Vector3[] playersPos;
    [HideInInspector] public Quaternion[] playerRots;

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

    public void DevCheckGameSelection() {
        if (Input.GetKey(KeyCode.LeftShift)) {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                // Red Light Green Light
                //devMinigameNumber = 0;
                SetDevMinigame(0);
                Debug.Log("RLGL Set");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2)) {
                // Reaction Time
                //devMinigameNumber = 1;
                SetDevMinigame(1);
                Debug.Log("RT Set");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3)) {
                // Sword in Stone
                //devMinigameNumber = 2;
                SetDevMinigame(2);
                Debug.Log("Sword Set");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha0)) {
                // No Minigame
                devMinigameNumber = -1;
                Debug.Log("Random Set");
            }
        }
        //SceneChanger.Instance.ChangeScene(devMinigameNumber + 4);
    }

    private void SetDevMinigame(int minigameNumber) {
        devMinigameNumber = minigameNumber;
        if (Input.GetKey(KeyCode.CapsLock)) {
            devMinigameNumber = -1;
            SceneChanger.Instance.ChangeScene(minigameNumber + 4);
        }
    }
}
