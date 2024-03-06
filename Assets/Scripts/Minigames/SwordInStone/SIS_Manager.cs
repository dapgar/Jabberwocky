using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SIS_Manager : MonoBehaviour {
    [Header("Gameplay Stuff")]
    [SerializeField]
    private int totalSwordPulls = 50;
    [SerializeField]
    private float maxPlayerStamina = 1f;
    /* Max height to pull sword before it comes out of rock */
    [SerializeField]
    private float maxPullLength = 0.6f;
    [SerializeField]
    private float staminaRegenRate = 0.25f;
    [SerializeField]
    private float staminaClickDrain = 0.05f;
    [SerializeField]
    private float gameWinMoveTime = 2f;

    [Header("GameObjects")]
    [SerializeField]
    private GameObject[] Swords;
    [SerializeField]
    private Image[] PlayerIcons;
    [SerializeField]
    private GameObject[] playerIconObjects;

    [Header("Pre Game Stuff")]
    [SerializeField]
    private GameObject preGameTimer;
    [SerializeField]
    private TMP_Text preGameTimerText;
    [SerializeField]
    private float gameStartTimer = 5f;
    private bool gameStarting = true;

    /* Amount that each click raises the sword */
    private float clickPullAmount;
    private bool bGameRunning = true;

    private List<SIS_Character> players;
    private int numPlayers;

    private void Start() {
        clickPullAmount = maxPullLength / totalSwordPulls;

        numPlayers = GameManager.instance ? GameManager.instance.numPlayers : 4;
        players = new List<SIS_Character>(numPlayers);
    }

    public void SetupPlayer(SIS_Character playa) {
        players.Add(playa);
        int playaIndex = players.IndexOf(playa);
        playa.SetupPlayer(playaIndex, maxPlayerStamina, staminaRegenRate, staminaClickDrain);
        playerIconObjects[playaIndex].SetActive(true); // not using stamina temp, just keep stamina icons hidden
    }

    private void Update() {
        if (gameStarting) {
            gameStartTimer -= Time.deltaTime;
            preGameTimerText.text = $"Game Starts In: {gameStartTimer.ToString("F1")}";
            if (gameStartTimer < 0) {
                gameStarting = false;
                bGameRunning = true;
                preGameTimer.gameObject.SetActive(false);
            }
        }
        else if (bGameRunning) {
            for (int i = 0; i < players.Count; i++) {
                //PlayerIcons[i].fillAmount = players[i].Stamina; // if we go back to stamina
                PlayerIcons[i].fillAmount = players[i].IsButtonPressed ? 1 : 0;
                if (players[i].CheckClick()) {
                    Vector3 newSwordPos = new Vector3(Swords[i].transform.position.x, Swords[i].transform.position.y, Swords[i].transform.position.z);
                    newSwordPos.y += clickPullAmount;
                    Swords[i].transform.position = newSwordPos;

                    CheckForWin(i);

                    break;
                }
            }
        }
    }

    private void CheckForWin(int playerIndex) {
        if (Swords[playerIndex].transform.position.y >= maxPullLength) {
            // If inside this IF check, a player has won, end game

            bGameRunning = false;

            StartCoroutine(MoveOnWinCoroutine(playerIndex));

            // This logic is wrapped in if check so we can test locally in this scene without getting error at end of game
            if (GameManager.instance) {
                int[] moveData = new int[GameManager.instance.numPlayers];
                for (int i = 0; i < players.Count; i++) {
                    moveData[i] = 0;
                }
                moveData[playerIndex] = GameManager.instance.diceRoll;
                GameManager.instance.MoveData(moveData);

                // back to board
                StartCoroutine(ReturnToBoardCoroutine());
            }
            else {
                Debug.Log("Game's Over - Must run from main Board Scene to return to board");
            }
        }
    }

    private IEnumerator ReturnToBoardCoroutine() {
        yield return new WaitForSeconds(3 + gameWinMoveTime);
        SceneManager.LoadScene(1);
    }

    private IEnumerator MoveOnWinCoroutine(int playerIndex) {
        float t = 0f;

        Vector3 startPosition = players[playerIndex].transform.position;
        Vector3 targetPosition = new Vector3(0, 0.85f, 0);

        Quaternion startRotation = players[playerIndex].transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(-20f, 180f, 0);

        // TODO: Rotate also by slerping

        while (t < gameWinMoveTime) {
            players[playerIndex].transform.position = Vector3.Slerp(startPosition, targetPosition, t / gameWinMoveTime);
            players[playerIndex].transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t / gameWinMoveTime);
            t += Time.deltaTime;
            yield return null;
        }

        // Ensure player is in exact correct position
        players[playerIndex].transform.position = targetPosition;
        players[playerIndex].transform.rotation = targetRotation;
    }
}
