using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SIS_Manager : MonoBehaviour {
    [Header("Gameplay Stuff")]
    [SerializeField]
    private int totalSwordPulls = 50;
    [SerializeField]
    private float playerClickCooldown = 1f;
    /* Max height to pull sword before it comes out of rock */
    [SerializeField]
    private float maxPullLength = 0.8f;
    [SerializeField]
    private float gameClickCooldown = 2f;

    [Header("GameObjects")]
    [SerializeField]
    private SIS_Character[] players;
    [SerializeField]
    private GameObject Sword;

    /* Amount that each click raises the sword */
    private float pullAmount;
    private bool bGameRunning = true;
     

    private void Start() {
        pullAmount = maxPullLength / totalSwordPulls;

        for (int i = 0; i < players.Length; i++) {
            players[i].SetupPlayer(playerClickCooldown, i);
        }

    }

    private void Update() {
        if (bGameRunning) {

            for (int i = 0; i < players.Length; i++) {
                if (players[i].CheckClick()) {
                    //StartCoroutine(GameClickCoroutine());

                    StartCoroutine(SwordPullCoroutine());

                    CheckForWin(i);

                    break;
                }
            }
        }
        else {
            // Game is over, should play end anim & do that stuff
        }
        
    }

    private void CheckForWin(int playerIndex) {
        if (Sword.transform.position.y >= maxPullLength) {
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

    private IEnumerator SwordPullCoroutine() {
        yield return new WaitForSeconds(1); // takes 1 sec for anim to get to sword pull state

        Vector3 newSwordTransform = Sword.transform.position;
        newSwordTransform.y += pullAmount;
        Sword.transform.position = newSwordTransform;
    }

/*    private IEnumerator GameClickCoroutine() {
        bClicksOnCooldown = true;

        yield return new WaitForSeconds(gameClickCooldown);

        bClicksOnCooldown = false;
    }*/

    private IEnumerator ReturnToBoardCoroutine() {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(1);
    }


}
