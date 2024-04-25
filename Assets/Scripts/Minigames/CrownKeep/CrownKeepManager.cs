using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CrownKeepManager : MonoBehaviour {
    private enum GameState {
        PreGame = 1,
        Game = 2,
        PostGame = 3,
    }
    private GameState gameState;

    private List<CrownKeepCharacter> players;
    private List<CrownKeepCharacter> playersInOrder;

    [SerializeField]
    private float gameTimer = 45f;
    [SerializeField]
    private TMP_Text timerText;

    private float preGameTimer = 2f;

    private int currentCrownHolder = -1;

    [SerializeField]
    private GameObject crown;

    private bool bReturningToBoard = false;

    private bool bPlayersCanMove = false;

    /* Scoring UI Stuff */
    [SerializeField]
    private TMP_Text[] scoreTimeTexts;
    [SerializeField]
    private Image[] playerImages;
    private Sprite[] playerIcons;

    public bool PlayersCanMove { get { return bPlayersCanMove; } }

    private void Start() {
        players = new List<CrownKeepCharacter>();
        playersInOrder = new List<CrownKeepCharacter>();
        gameState = GameState.PreGame;

        foreach (TMP_Text text in scoreTimeTexts) {
            text.text = "0s";
        }
        playerIcons = PlayerConfigurationManager.Instance.GetUsedPlayerIcons();
        for (int i = 0; i < playerIcons.Length; i++) {
            playerImages[i].sprite = playerIcons[i];
        }
    }
    public void GetPlayers(CrownKeepCharacter player) {
        players.Add(player);
        playersInOrder.Add(player);
        player.playerSprite = playerIcons[players.IndexOf(player)];
    }

    private void UpdateLeaderboard() {
        playersInOrder = playersInOrder.OrderByDescending(p => p.CrownTime).ToList();
        for (int i = 0; i < playersInOrder.Count; i++) {
            scoreTimeTexts[i].text = playersInOrder[i].CrownTime.ToString("F1") + "s";
            playerImages[i].sprite = playersInOrder[i].playerSprite;
        }
    }

    private void Update() {
        if (gameState == GameState.Game) {
            gameTimer -= Time.deltaTime;
            if (gameTimer <= 0f) {
                gameTimer = 0f;
                gameState = GameState.PostGame;
                bPlayersCanMove = false;
            }
            timerText.text = $"Time Remaining: {gameTimer.ToString("F1")}";
            UpdateLeaderboard();
        }
        else if (gameState == GameState.PreGame) {
            // In pregame
            preGameTimer -= Time.deltaTime;
            if (preGameTimer <= 0f) {
                gameState = GameState.Game;
                preGameTimer = 0f;
                bPlayersCanMove = true;
            }
            timerText.text = $"Begins In: {preGameTimer.ToString("F1")}";

        }
        else {
            if (bReturningToBoard) return;
            
            // In PosGame
            int winningPlayerIndex = -1;
            for (int i = 0; i < players.Count; i++) {
                if (winningPlayerIndex == -1 && players[i].CrownTime > 0f) {
                    winningPlayerIndex = i;
                }
                else if (winningPlayerIndex != -1 && players[i].CrownTime > players[winningPlayerIndex].CrownTime) {
                    winningPlayerIndex = i;
                }
            }
            Debug.Log("Winning player index is: " + winningPlayerIndex);

            if (GameManager.instance) {
                int[] moveData = new int[GameManager.instance.numPlayers];
                for (int i = 0; i < moveData.Length; i++) {
                    moveData[i] = 0;
                }
                if (winningPlayerIndex != -1) moveData[winningPlayerIndex] = GameManager.instance.diceRoll;
                GameManager.instance.MoveData(moveData);

                // Go Back to board
                StartCoroutine(ReturnToBoardCoroutine());
            }

        }
    }

    private IEnumerator ReturnToBoardCoroutine() {
        bReturningToBoard = true;
        yield return new WaitForSeconds(2.5f);
        SceneChanger.Instance.ChangeScene(1);
    }

    public void OnCrownTouched(CrownKeepCharacter player) {
        Debug.Log("Crown touched");
        int index = players.IndexOf(player);

        // Initial Crown Pickup currentCrownHolder == -1, so guy who picks it up skips other checks
        if (currentCrownHolder == -1) {
            currentCrownHolder = index;
            player.GotCrown();
            AttachCrownToPlayer(player);
            crown.GetComponent<Animator>().enabled = false;
            return;
        }

        if (index == -1 || index == currentCrownHolder || !players[currentCrownHolder].CanGetStolen()) {
            return;
        }

        players[currentCrownHolder].CrownStolen();
        currentCrownHolder = index;
        player.GotCrown();
        AttachCrownToPlayer(player);
    }

    private void AttachCrownToPlayer(CrownKeepCharacter player) {
        crown.transform.parent = player.transform;
        crown.transform.localPosition = new Vector3(0f, 0.6f, 0f);
        crown.transform.localRotation = Quaternion.identity;
    }

}
