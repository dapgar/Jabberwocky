using System.Collections;
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
        if (bGameRunning) {
            for (int i = 0; i < players.Length; i++) {
                if (players[i].Exhausted) {
                    PlayerIcons[i].enabled = false;
                }
                else {
                    PlayerIcons[i].enabled = true;
                }
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


}
