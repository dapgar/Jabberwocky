using Cinemachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using CommandTerminal;

public class BoardManager : MonoBehaviour {
    public static BoardManager instance;

    private List<GameObject> playerSpanws = new List<GameObject>();

    public List<StoneScript> players;
    public List<StoneScript> playerRankings;
    public List<Image> playerIcons;
    public List<Sprite> playerSprites;
    private Sprite[] charIcons; // icons that are used by the characters
    //public GameObject[] crowns;

    public CinemachineVirtualCamera cam;
    private Vector3 camDefaultPos;
    private Quaternion camDefaultRot;

    public RouteScript route;

    private bool isEnding;

    private int itemTargetedMoveBackAmount = 3;
    enum BoardState {
        Turn = 1,
        Idle = 2,
    }

    enum TurnState {
        Moving = 1,
        PostMove = 2,
        Item = 3,
    }

    private BoardState boardState = BoardState.Idle;
    private TurnState turnState = TurnState.Moving;
    private int currentPlayer = 0;
    private bool[] gotItemAlready = new bool[4];

    [SerializeField]
    private GameObject itemMovePlayerBackUI;

    [SerializeField]
    private GameObject itemSelectPlayerToSwapWithUI;

    public bool itemUIOpen = false;

    private bool[] movedThisTurn;

    void Start() {
        movedThisTurn = new bool[players.Count];
        for (int i = 0; i < movedThisTurn.Length; i++) {
            movedThisTurn[i] = false;
        }
        currentPlayer = 0;
        for (int i = 0; i < gotItemAlready.Length; i++) {
            gotItemAlready[i] = false;
        }

        if (instance == null) instance = this;

        for (int i = 0; i < GameManager.instance.playersPos.Length; i++) {
            players[i].routePos = GameManager.instance.routeData[i];
            if (GameManager.instance.playersPos[i] != Vector3.zero) players[i].transform.position = GameManager.instance.playersPos[i];
            if (GameManager.instance.playerRots[i] != Quaternion.identity) players[i].transform.rotation = GameManager.instance.playerRots[i];
        }

        foreach (StoneScript p in players) {
            p.LookAtCamera();
        }

        camDefaultPos = new Vector3(2.7f, 8.02f, 26.3f);
        camDefaultRot = Quaternion.Euler(38.687f, 180, 0);

        GameManager.instance.playersMoving = true;

        // Board UI
        charIcons = PlayerConfigurationManager.Instance.GetUsedPlayerIcons();
        for (int i = 0; i < playerIcons.Count; i++) {
            playerIcons[i].sprite = charIcons[i];
        }
        //playerIcons[0].sprite = playerSprites[playerRankings[0].stoneID - 1];
        //playerIcons[1].sprite = playerSprites[playerRankings[1].stoneID - 1];
        //playerIcons[2].sprite = playerSprites[playerRankings[2].stoneID - 1];
        //playerIcons[3].sprite = playerSprites[playerRankings[3].stoneID - 1];
    }

    public void SavePlayersPos() {
        for (int playerIndex = 0; playerIndex < players.Count; playerIndex++) {
            GameManager.instance.routeData[playerIndex] = players[playerIndex].routePos;
            GameManager.instance.playersPos[playerIndex] = players[playerIndex].transform.position;
            GameManager.instance.playerRots[playerIndex] = players[playerIndex].transform.rotation;
        }
        
    }

    private void MovePlayer(int playerIndex, int spaces, bool bSkipAnim) {
        StartCoroutine(players[playerIndex].MovePlayer(spaces, bSkipAnim));
    }

    public void DevMovePlayer(int playerNum, int spaces) {
        if (playerNum > players.Count) {
            Debug.Log($"Player num {playerNum} doesn't exist");
            return;
        }
        currentPlayer = playerNum - 1;
        MovePlayer(currentPlayer, spaces, true);
    }

    private void ItemMovePlayerBackwards(int playerID) {
        // Player num is INDEX, 0-3
        // Move Player Backwards (Targeted, Dynamic Amount)
        MovePlayer(playerID, -itemTargetedMoveBackAmount, false);
        itemMovePlayerBackUI.gameObject.SetActive(false);
        itemUIOpen = false;

        currentPlayer++;
        boardState = BoardState.Idle;
    }

    private void ItemMoveAllBack() {
        for (int i = 0; i < players.Count; i++) {
            if (i == currentPlayer) continue;
            MovePlayer(i, -2, false);
        }
        currentPlayer++;
        boardState = BoardState.Idle;
    }

    private void ItemDoubleCurrentRoll() {
        MovePlayer(currentPlayer, GameManager.instance.diceRoll, false);
        currentPlayer++;
        boardState = BoardState.Idle;
    }

    private void ItemSwapWithPlayerTargeted(int playerNumToSwapWith) {
        // Player num is index, but rather player num,
        // so values are 0-3
        int newRoutePos = players[playerNumToSwapWith].routePos;
        Vector3 newPos = players[playerNumToSwapWith].transform.position;
        Quaternion newRot = players[playerNumToSwapWith].transform.rotation;
        
        players[playerNumToSwapWith].routePos = players[currentPlayer].routePos;
        players[playerNumToSwapWith].transform.position = players[currentPlayer].transform.position;
        players[playerNumToSwapWith].transform.rotation = players[currentPlayer].transform.rotation;

        players[currentPlayer].routePos = newRoutePos;
        players[currentPlayer].transform.position = newPos;
        players[currentPlayer].transform.rotation = newRot;

        gotItemAlready[playerNumToSwapWith] = true;

        itemSelectPlayerToSwapWithUI.gameObject.SetActive(false);
        itemUIOpen = false;
        
        SavePlayersPos();

        currentPlayer++;
        boardState = BoardState.Idle;
    }

    public void ItemOpenMoveBackPlayerUI() {
        PlayerInput input = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray()[currentPlayer].Input;
        input.uiInputModule = itemMovePlayerBackUI.GetComponentInChildren<InputSystemUIInputModule>();

        Sprite[] playerIcons = PlayerConfigurationManager.Instance.GetUsedPlayerIcons();
        Button[] buttons = itemMovePlayerBackUI.GetComponentsInChildren<Button>();
        int btnIndex = 0;
        for (int i = 0; i < playerIcons.Length; i++) {
            if (i == currentPlayer) continue;
            buttons[btnIndex].image.sprite = playerIcons[i];
            int toMoveBack = i;
            buttons[btnIndex].onClick.AddListener(delegate { ItemMovePlayerBackwards(toMoveBack); });
            btnIndex++;
        }
        itemMovePlayerBackUI.gameObject.SetActive(true);
        itemUIOpen = true;
    }

    private void ItemOpenSwapPlayerUI() {
        PlayerInput input = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray()[currentPlayer].Input;
        input.uiInputModule = itemSelectPlayerToSwapWithUI.GetComponentInChildren<InputSystemUIInputModule>();

        Sprite[] playerIcons = PlayerConfigurationManager.Instance.GetUsedPlayerIcons();
        Button[] buttons = itemSelectPlayerToSwapWithUI.GetComponentsInChildren<Button>();
        int btnIndex = 0;
        for (int i = 0; i < playerIcons.Length; i++) {
            if (i == currentPlayer) continue;
            buttons[btnIndex].image.sprite = playerIcons[i];
            int currentIndex = i;
            buttons[btnIndex].onClick.AddListener(delegate { ItemSwapWithPlayerTargeted(currentIndex); });
            btnIndex++;
        }
        itemSelectPlayerToSwapWithUI.gameObject.SetActive(true);
        itemUIOpen = true;
    }

    public void ActivateItem(int itemNum, GameObject itemDiceObj) {
        //Debug.Log("Activate item, itemNum = " + itemNum);
        itemDiceObj.SetActive(false);
        switch (itemNum) {
            case 1:
                // Move Player Backwards (Targeted, Dynamic Amount)
                Debug.Log("Item Dice: Move Player Backwards (Targeted, Dynamic Amount)");
                ItemOpenMoveBackPlayerUI();
                break;
            case 2:
                // Move Everyone (but you) Backwards 2
                Debug.Log("Item Dice: Move Everyone (but you) Backwards 2");
                ItemMoveAllBack();
                break;
            case 3:
                // Move Ahead (Double Your Roll) (Yourself, Dynamic Amount)
                Debug.Log("Item Dice: Move Ahead (Double Your Roll) (Yourself, Dynamic Amount)");
                ItemDoubleCurrentRoll();
                break;
            case 4:
                // Swap Positions (Targeted or Random)
                Debug.Log("Item Dice: Swap Positions (Targeted or Random)");
                ItemOpenSwapPlayerUI();
                break;
            case 5:
                // Move Ahead (Double Your Roll) (Yourself, Dynamic Amount) (2nd chance)
                Debug.Log("Item Dice: Move Ahead (Double Your Roll) (Yourself, Dynamic Amount) (2nd chance)");
                ItemDoubleCurrentRoll();
                break;
            case 6:
                // Move Player Backwards (Targeted, Dynamic Amount) (2nd chance)
                Debug.Log("Item Dice: Move Player Backwards (Targeted, Dynamic Amount) (2nd chance)");
                ItemOpenMoveBackPlayerUI();
                break;
            default:
                currentPlayer++;
                boardState = BoardState.Idle;
                break;
        }
    }

    private void Update() {
        switch (boardState) {
            case BoardState.Turn:
                switch (turnState) {
                    case TurnState.Moving:
                        // Moves player until they're done with their movement
                        StoneScript player = players[currentPlayer];
                        if (player.MoveFinished) {
                            turnState = TurnState.PostMove;
                        }
                        else if (!player.isMoving) {
                            if (GameManager.instance.moveData[player.stoneID - 1] > 0) {
                                movedThisTurn[currentPlayer] = true;
                            }
                            StartCoroutine(player.MovePlayer(GameManager.instance.moveData[player.stoneID - 1]));
                        }
                        break;
                    case TurnState.PostMove:
                        GameManager.instance.routeData[currentPlayer] = players[currentPlayer].routePos;
                        // Checks if player is on an item node
                        if (!gotItemAlready[currentPlayer] && movedThisTurn[currentPlayer] && route.childNodeList[players[currentPlayer].routePos].GetComponent<Node>().isItemSpace) {
                            turnState = TurnState.Item;
                        }
                        else {
                            currentPlayer++;
                            boardState = BoardState.Idle;
                        }
                        break;
                    case TurnState.Item:
                        // TO DISABLE ITEM DICE, UNCOMMENT THESE FOLLOWING 3 LINES
                        /*currentPlayer++;
                        boardState = BoardState.Idle;
                        break;*/

                        if (!gotItemAlready[currentPlayer]) StartCoroutine(ItemDiceManager.Instance.RollDice());
                        gotItemAlready[currentPlayer] = true;
                        break;
                }
                break;
            case BoardState.Idle:
                if (currentPlayer < players.Count) {
                    boardState = BoardState.Turn;
                    turnState = TurnState.Moving;
                }
                else {
                    // Clean up & save info at end of round
                    SavePlayersPos();
                    for (int i = 0; i < players.Count; i++) {
                        GameManager.instance.moveData[i] = 0;
                        gotItemAlready[i] = false;
                    }
                    // Setting playersMoving = false ends the round
                    GameManager.instance.playersMoving = false;
                }
                break;
        }

        // BELOW CAN PROB GET MOVED INTO FSM AND ONLY CALLED AFTER EACH MOVE IN FUTURE
        playerRankings = players.OrderByDescending(player => player.routePos).ToList();

        // Win con
        foreach (StoneScript player in players) {
            if (player.routePos == route.childNodeList.Count - 1 && !isEnding) {
                // This player won!
                GameManager.instance.playerRankings.Add(playerRankings[0].stoneID);
                GameManager.instance.playerRankings.Add(playerRankings[1].stoneID);
                GameManager.instance.playerRankings.Add(playerRankings[2].stoneID);
                GameManager.instance.playerRankings.Add(playerRankings[3].stoneID);
                SceneChanger.Instance.ChangeScene(3);
                isEnding = true;
            }
        }

        for (int i = 0; i < playerIcons.Count; i++)
        {
            if (playerRankings[i] != null)
            {
                playerIcons[i].sprite = charIcons[playerRankings[i].stoneID-1];
            }
            else
            {
                playerIcons[i].sprite = charIcons[i];
            }
        }
        //// Board UI
        //playerIcons[0].sprite = playerSprites[playerRankings[0].stoneID - 1];
        //playerIcons[1].sprite = playerSprites[playerRankings[1].stoneID - 1];
        //playerIcons[2].sprite = playerSprites[playerRankings[2].stoneID - 1];
        //playerIcons[3].sprite = playerSprites[playerRankings[3].stoneID - 1];
    }

    public void SetupPlayer(GameObject player)
    {
        int index = player.GetComponent<PlayerInputHandler>().GetIndex();
        StoneScript stoneScript = player.GetComponent<StoneScript>();

        players[index] = stoneScript;
        players[index].playerAnim = player.GetComponentInChildren<Animator>();

        switch (index)
        {
            case 0:
                players[index].nodeOffset = new Vector3(-.3f, 0f, .3f);
                break;
            case 1:
                players[index].nodeOffset = new Vector3(.3f, 0f, .3f);
                break;
            case 2:
                players[index].nodeOffset = new Vector3(-.3f, 0f, -.3f);
                break;
            case 3:
                players[index].nodeOffset = new Vector3(.3f, 0f, -.3f);
                break;
            default:
                players[index].nodeOffset = Vector3.zero;
                break;
        }

        players[index].stoneID = index + 1;
        players[index].currentRoute = route;
        players[index].routePos = GameManager.instance.routeData[index];
        if (GameManager.instance.playersPos[index] == Vector3.zero)
        {
            GameManager.instance.playersPos[index] = players[index].currentRoute.childNodeList[players[index].routePos].position + players[index].nodeOffset;
        }
        players[index].transform.position = GameManager.instance.playersPos[index];
        players[index].transform.rotation = GameManager.instance.playerRots[index];

        players[index].LookAtCamera();
    }

    /* Deprecated
    IEnumerator UpdateBoard() {
         Used to disable dice roll during player moves
        GameManager.instance.playersMoving = true;

        yield return new WaitForSeconds(3f);

        // Moves players after each game.
        foreach (StoneScript player in players) {
            if ((GameManager.instance.moveData[player.stoneID - 1] > 0)) {
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
    */

    private void PointCam(Transform target) {
        cam.LookAt = target;
        cam.m_Lens.FieldOfView = 30;
    }

    private void ResetCam() {
        cam.LookAt = null;
        cam.transform.SetPositionAndRotation(camDefaultPos, camDefaultRot);
        cam.m_Lens.FieldOfView = 70;
    }

    [RegisterCommand(Help = "Set currentPlayer index", MinArgCount = 1, MaxArgCount = 1)]
    static void SetCurrentPlayer(CommandArg[] args) {
        instance.currentPlayer = args[0].Int;
    }
}


