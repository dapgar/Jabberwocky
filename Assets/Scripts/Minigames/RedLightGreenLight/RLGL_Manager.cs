using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RLGL_Manager : MonoBehaviour {
    [Header("UI Stuff")]
    [SerializeField]
    private GameObject preGameTimer;
    [SerializeField]
    private TMP_Text timerText;
    [SerializeField]
    private GameObject GameTimer;
    [SerializeField]
    private TMP_Text preGameTimerText;
    private float gameStartTimer = 5f;

    [Header("Gameplay Stuff")]
    [SerializeField]
    private float acceleration = 2f;
    [SerializeField]
    private float deceleration = 4f;
    [SerializeField]
    private float maxSpeed = 5f;

    [SerializeField]
    private float gameTime;
    [SerializeField]
    private float finishLineZ;
    [SerializeField]
    private RLGL_Character[] players;
    
    /* Time that the timer decreases by every time a player finishes */
    [SerializeField]
    private float finishTimerDecreaseAmount = 2f;

    [SerializeField]
    private int firstFinishBonus = 1;

    [Header("Light Stuff")]
    [SerializeField]
    private Transform lightIndicator;
    [SerializeField]
    private float rotationTime = 0.75f;
    [SerializeField]
    private float maxRedLightDuration = 4f;
    [SerializeField]
    private float minRedLightDuration = 1.5f;
    [SerializeField]
    private float maxGreenLightDuration = 5f;
    [SerializeField]
    private float minGreenLightDuration = 2f;
    /* Higher reaction time buffer makes game easier */
    [SerializeField]
    private float reactionTimeBuffer = 0.75f;

    private float timer = 0f;

    private bool isLightRed = true;
    private bool lightJustSwitched = false;
    private float reactionTimeTimer = 0f;

    private bool runLightCoroutine = true;  // For cleanup, set this to false to stop looping coroutine
    private bool gameRunning = true;

    private int playingPlayers;

    private int firstFinishIndex = -1;

    private void Start() {
        StartCoroutine(LightController());

        for (int i = 0; i < players.Length; i++) {
            players[i].SetupPlayer(i, players.Length, acceleration, deceleration, maxSpeed);
        }
        playingPlayers = players.Length;

        timer = gameTime;
    }

    private void Update() {
        if (gameStartTimer > 0) {
            gameStartTimer -= Time.deltaTime;
            preGameTimerText.text = $"Game Starts In: {gameStartTimer.ToString("F1")}";
        }
        else if (gameRunning) {
            timer -= Time.deltaTime;
            timerText.text = timer.ToString("F1");
            if (timer <= 0.0f) {
                GameOver();
            }

            for (int i = 0; i < players.Length; i++) {
                if (!players[i].IsFinished && players[i].IsAlive) {
                    players[i].Move();
                    if (players[i].CheckFinish(finishLineZ)) {
                        playingPlayers--;
                        timer -= finishTimerDecreaseAmount;
                        if (firstFinishIndex == -1) firstFinishIndex = i;
                    }
                }
            }

            if (lightJustSwitched) {
                reactionTimeTimer += Time.deltaTime;
                if (reactionTimeTimer > reactionTimeBuffer) {
                    reactionTimeTimer = 0;
                    lightJustSwitched = false;
                }
            }
            if (isLightRed && !lightJustSwitched) {
                foreach (RLGL_Character player in players) {
                    if (player.IsAlive && player.IsMoving) {
                        player.Kill();
                        playingPlayers--;
                    }
                }
            }

           if (playingPlayers == 0) {
                GameOver();
            }
        }
    }

    private void GameOver() {
        if (!gameRunning) return;
        gameRunning = false;
        runLightCoroutine = false;

        int[] moveData = new int[GameManager.instance.numPlayers];
        int spacesToMove = GameManager.instance.diceRoll;

        for (int i = 0; i < players.Length; i++) {
            if (players[i].IsAlive) players[i].Kill();
            moveData[i] = players[i].IsFinished ? spacesToMove : 0;
        }
        if (firstFinishIndex != -1) moveData[firstFinishIndex] += firstFinishBonus;

        // back to board
        StartCoroutine(ReturnToBoardCoroutine());
    }

    private IEnumerator ReturnToBoardCoroutine() {
        yield return new WaitForSeconds(5);

        SceneManager.LoadScene(0);
    }

    private IEnumerator LightController() {
        while (runLightCoroutine) {
            float duration = isLightRed ? Random.Range(minRedLightDuration, maxRedLightDuration) : Random.Range(minGreenLightDuration, maxGreenLightDuration);

            //Debug.Log($"Light is {(isLightRed ? "Red" : "Green")} for {duration} seconds");

            StartCoroutine(RotateCoroutine(isLightRed ? 0 : 180));

            yield return new WaitForSeconds(duration);

            isLightRed = !isLightRed;
            lightJustSwitched = true;
        }
    }

    private IEnumerator RotateCoroutine(float targetYRot) {
        float elapsed_time = 0f;
        Quaternion startRotation = lightIndicator.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, targetYRot, 0);

        while (elapsed_time < rotationTime) {
            lightIndicator.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed_time / rotationTime);
            elapsed_time += Time.deltaTime;
            yield return null;
        }

        // Ensure object is exactly 180 degrees or 0 degrees
        lightIndicator.transform.rotation = targetRotation;
    }

}
