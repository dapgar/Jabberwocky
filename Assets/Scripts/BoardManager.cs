using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;

    public List<StoneScript> players;
    public List<StoneScript> playerRankings;
    public List<Image> playerIcons;
    public List<Sprite> playerSprites;
    //public GameObject[] crowns;

    public CinemachineVirtualCamera cam;
    private Vector3 camDefaultPos;
    private Quaternion camDefaultRot;

    public RouteScript route;

    private bool isEnding;

    enum BoardState {
        Turn = 1,
        Idle = 2,
    }

    enum TurnState {
        Moving = 1,
        Item = 2,
    }

    private BoardState boardState = BoardState.Idle;
    private TurnState turnState = TurnState.Moving;
    private int currentPlayer = 0;
    private bool[] moveTurnComplete = new bool[4];

    void Start()
    {
        currentPlayer = 0;
        for (int i = 0; i < moveTurnComplete.Length; i++) {
            moveTurnComplete[i] = false;
        }

        if (instance == null) instance = this;

        for (int i = 0; i < GameManager.instance.playersPos.Length; i++) {
            players[i].routePos = GameManager.instance.routeData[i];
            if (GameManager.instance.playersPos[i] != Vector3.zero) players[i].transform.position = GameManager.instance.playersPos[i];
            if (GameManager.instance.playerRots[i] != Quaternion.identity)  players[i].transform.rotation = GameManager.instance.playerRots[i];
        }

        // Old using routes instead of Vector & Quaternion
        /*for (int i = 0; i < GameManager.instance.routeData.Length; i++)
        {
            if (GameManager.instance.routeData[i] != 0)
            {
                players[i].routePos = GameManager.instance.routeData[i];
                players[i].transform.position = route.childNodeList[i].position;
            }
        }*/

        foreach (StoneScript p in players) {
            p.LookAtCamera();
        }

        camDefaultPos = new Vector3(2.7f, 8.02f, 26.3f);
        camDefaultRot = Quaternion.Euler(38.687f, 180, 0);

        //StartCoroutine(UpdateBoard());
    }

    public void DevMovePlayer(int playerNum, int spaces) {
        if (playerNum > players.Count) {
            Debug.Log($"Player num {playerNum} doesn't exist");
            return;
        }
        Debug.Log($"Moving player {playerNum} {spaces} spaces");
        StartCoroutine(players[playerNum - 1].MovePlayer(spaces, true));
        GameManager.instance.routeData[playerNum - 1] = players[playerNum - 1].routePos;
    }
    
    private void Update() {
        switch (boardState) {
            case BoardState.Turn:
                switch (turnState) {
                    case TurnState.Moving:
                        // Moves player until they're done with their movement
                        StoneScript player = players[currentPlayer];
                        if (!player.isMoving) StartCoroutine(player.MovePlayer(GameManager.instance.moveData[player.stoneID - 1]));
                        if (player.MoveFinished) {
                            turnState = TurnState.Item;
                            GameManager.instance.routeData[currentPlayer] = players[currentPlayer].routePos;
                        }
                        break;
                    case TurnState.Item:
                        // Movement is done, check if player is on an item space
                        if (!moveTurnComplete[currentPlayer] && false) {
                            // PLAYER ON ITEM SPACE, DO THAT
                            // TODO: NEED ACTUAL CONDITION ABOVE
                            // CHECK FOR IF PLAYER ON ITEM SPACE
                        }
                        else {
                            currentPlayer++;
                            boardState = BoardState.Idle;
                        }
                        moveTurnComplete[currentPlayer] = true;
                        break;
                }
                break;
            case BoardState.Idle:
                if (currentPlayer < players.Count) {
                    boardState = BoardState.Turn;
                    turnState = TurnState.Moving;
                    GameManager.instance.playersMoving = true;
                }
                else {
                    // Clean up at end of round
                    for (int i = 0; i < players.Count; i++) {
                        GameManager.instance.playersPos[i] = players[i].transform.position;
                        GameManager.instance.playerRots[i] = players[i].transform.rotation;
                        GameManager.instance.moveData[i] = 0;
                    }
                    GameManager.instance.playersMoving = false;
                    // TODO: End Round
                    // NOTE: I think setting playersMoving = false does end the round
                }
                break;
        }

        // BELOW CAN PROB GET MOVED INTO FSM AND ONLY CALLED AFTER EACH MOVE IN FUTURE
        playerRankings = players.OrderByDescending(player => player.routePos).ToList();
        
        // Win con
        foreach (StoneScript player in players)
        {
            if (player.routePos == route.childNodeList.Count - 1 && !isEnding)
            {
                // This player won!
                GameManager.instance.playerRankings.Add(playerRankings[0].stoneID);
                GameManager.instance.playerRankings.Add(playerRankings[1].stoneID);
                GameManager.instance.playerRankings.Add(playerRankings[2].stoneID);
                GameManager.instance.playerRankings.Add(playerRankings[3].stoneID);
                SceneChanger.Instance.ChangeScene(3);
                isEnding = true;
            }
        }

        // Board UI
        playerIcons[0].sprite = playerSprites[playerRankings[0].stoneID - 1];
        playerIcons[1].sprite = playerSprites[playerRankings[1].stoneID - 1];
        playerIcons[2].sprite = playerSprites[playerRankings[2].stoneID - 1];
        playerIcons[3].sprite = playerSprites[playerRankings[3].stoneID - 1];
    }

    /* Deprecated */
    IEnumerator UpdateBoard()
    {
        /* Used to disable dice roll during player moves */
        GameManager.instance.playersMoving = true;

        yield return new WaitForSeconds(3f);
       
        // Moves players after each game.
        foreach(StoneScript player in players)
        {
            if ((GameManager.instance.moveData[player.stoneID - 1] > 0))
            {
                StartCoroutine(player.Move(GameManager.instance.moveData[player.stoneID - 1]));
                PointCam(player.transform);
                while (player.isMoving) { yield return null; }
                ResetCam();
                yield return new WaitForSeconds(1f);
            }
            GameManager.instance.routeData[player.stoneID - 1] = player.routePos;
            //GameManager.instance.playersPos[player.stoneID - 1] = player.transform.position;
        }
        GameManager.instance.playersMoving = false;
        for (int i = 0; i < GameManager.instance.moveData.Length; i++) GameManager.instance.moveData[i] = 0;

        // Used for storing prev board pos / rot
        // Can clean this up AFTER MVI - don't wanna mess smth up rn lol
        for (int i = 0; i < players.Count; i++) {
            GameManager.instance.playersPos[i] = players[i].transform.position;
            GameManager.instance.playerRots[i] = players[i].transform.rotation;
        }
    }

    private void PointCam(Transform target)
    {
        cam.LookAt = target;
        cam.m_Lens.FieldOfView = 30;
    }

    private void ResetCam()
    {
        cam.LookAt = null;
        cam.transform.SetPositionAndRotation(camDefaultPos, camDefaultRot);
        cam.m_Lens.FieldOfView = 70;
    }
}


