using System.Collections;
using UnityEngine;
using TMPro;

public class RLGL_Manager : MonoBehaviour {
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
    [SerializeField]
    private TMP_Text timerText;
    /* Time that the timer decreases by every time a player finishes */
    [SerializeField]
    private float finishTimerDecreaseAmount = 2f;

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

    private void Start() {
        StartCoroutine(LightController());

        for (int i = 0; i < players.Length; i++) {
            players[i].SetupPlayer(i, players.Length, acceleration, deceleration, maxSpeed);
        }
        playingPlayers = players.Length;

        timer = gameTime;
    }

    private void Update() {
        if (gameRunning) {
            timer -= Time.deltaTime;
            timerText.text = timer.ToString("F1");
            if (timer <= 0.0f) {
                GameOver(true);
            }

            foreach (RLGL_Character player in players) {
                if (!player.IsFinished && player.IsAlive) {
                    player.Move();
                    if (player.CheckFinish(finishLineZ)) {
                        playingPlayers--;
                        timer -= finishTimerDecreaseAmount;
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
                GameOver(false);
            }
        }
    }

    private void GameOver(bool bTimeRanOut) {
        if (!gameRunning) return;
        gameRunning = false;
        runLightCoroutine = false;

        if (bTimeRanOut) {
            Debug.Log("Game Over: Time ran out");
            foreach (RLGL_Character player in players) {
                if (player.IsFinished) {
                    // Handle giving the winners their rewards
                    Debug.Log($"Player {player.PlayerIndex + 1} finished. They can move forward.");
                }
                else if (player.IsAlive) player.Kill();
                
            }
        }
        else {
            // This 'else' triggers IF:
            // - All players are dead
            // - All players have finished
            // - All players are dead or finished

            bool allDead = true;
            bool allFinished = true;
            foreach (RLGL_Character player in players) {
                if (player.IsAlive) allDead = false;
                if (!player.IsFinished) allFinished = false;
            }
            if (allDead) {
                Debug.Log("Game Over: Everyone's Dead");
            }
            else if (allFinished) {
                Debug.Log("Game Over: Everyone Finished, you can all move forward");
            }
            else {
                Debug.Log("Game Over: All players either died or finished.");
                foreach (RLGL_Character player in players) {
                    // Handle giving the winning players movement
                    if (player.IsFinished) Debug.Log($"Player {player.PlayerIndex + 1} finished. They can move forward.");
                }
            }
        }
        
            
        
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
