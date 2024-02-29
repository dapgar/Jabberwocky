using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public int numPlayers = 4;
    [HideInInspector] public int[] moveData;
    [HideInInspector] public int[] routeData;
    [HideInInspector] public Vector3[] playersPos;
    [HideInInspector] public Quaternion[] playerRots;

    [HideInInspector] public int diceRoll;

    [HideInInspector] public bool playersMoving;

    [HideInInspector] public int devMinigameNumber = -1;

    private void Awake() {
        DontDestroyOnLoad(transform.gameObject);
        moveData = new int[numPlayers];
        routeData = new int[numPlayers];
        playersPos = new Vector3[numPlayers];
        playerRots = new Quaternion[numPlayers];

        playersMoving = false;
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
                devMinigameNumber = 0;
                Debug.Log("RLGL Set");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2)) {
                // Reaction Time
                devMinigameNumber = 1;
                Debug.Log("RT Set");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3)) {
                // Sword in Stone
                devMinigameNumber = 2;
                Debug.Log("Sword Set");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha0)) {
                // No Minigame
                devMinigameNumber = -1;
                Debug.Log("Random Set");
            }
        }
    }
}
