using System.Collections;
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

    [Header("GameObjects")]
    [SerializeField]
    private SIS_Character[] players;
    [SerializeField]
    private GameObject[] Swords;
    [SerializeField]
    private Image[] PlayerIcons;

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
     

    private void Start() {
        clickPullAmount = maxPullLength / totalSwordPulls;

        for (int i = 0; i < players.Length; i++) {
            players[i].SetupPlayer(i, maxPlayerStamina, staminaRegenRate, staminaClickDrain);
        }

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
            for (int i = 0; i < players.Length; i++) {
                PlayerIcons[i].fillAmount = players[i].Stamina;
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
            // Game is over, player {playerIndex} won

            // Debug.Log($"Winner is player {playerIndex + 1}");
            bGameRunning = false;

            //Swords[playerIndex].transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

            StartCoroutine(MoveOnWinCoroutine(playerIndex));

            // This logic is wrapped in if check so we can test locally in this scene without getting error at end of game
            if (GameManager.instance) {
                int[] moveData = new int[GameManager.instance.numPlayers];
                for (int i = 0; i < players.Length; i++) {
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
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(1);
    }

    private IEnumerator MoveOnWinCoroutine(int playerIndex) {
        float t = 0f;
        float moveTime = 3f;

        Vector3 startPosition = players[playerIndex].transform.position;
        Vector3 targetPosition = new Vector3(0, 0.85f, 0);

        // TODO: Rotate also by slerping

        while (t < moveTime) {
            players[playerIndex].transform.position = Vector3.Slerp(startPosition, targetPosition, t / moveTime);
            t += Time.deltaTime;
            yield return null;
        }

        // Ensure player is in exact correct position
        players[playerIndex].transform.position = targetPosition;
        players[playerIndex].transform.rotation = Quaternion.Euler(-20f, 180f, 0f);
    }

}
