using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RLGL_Manager : MonoBehaviour {
    [Header("UI Stuff")]
    [SerializeField]
    private GameObject preGameTimer;
    [SerializeField]
    private TMP_Text timerText;
    [SerializeField]
    private GameObject gameTimer;
    [SerializeField]
    private TMP_Text preGameTimerText;
    [SerializeField]
    private float gameStartTimer = 5f;
    [SerializeField]
    private Image[] playerIcons;

    [Header("Gameplay Stuff")]
    [SerializeField]
    private float acceleration = 2f;
    [SerializeField]
    private float deceleration = 4f;
    [SerializeField]
    private float maxSpeed = 5f;
    [SerializeField]
    private float pushBackAmount = 1f;
    [SerializeField]
    private float pushBackSpeed = 2f;
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

    [Header("Camera Follow Stuff")]
    [SerializeField]
    private float followSpeed = 5f;
    [SerializeField]
    private float minFollowDistance = 3.5f;
    [SerializeField]
    private float maxFollowDistance = 7f;
    [SerializeField]
    private Camera followCam;
    [SerializeField]
    private float maxFollowZ = 2f;

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

    private float progressMinX = -225;
    private float progressMaxX = 225;

    private bool isLightRed = true;
    private bool lightJustSwitched = false;
    private float reactionTimeTimer = 0f;

    private bool runLightCoroutine = true;  // For cleanup, set this to false to stop looping coroutine
    private bool gameRunning = false;

    private int playingPlayers;

    private int firstFinishIndex = -1;

    private bool gameStarting = true;

    private void Start() {
        for (int i = 0; i < players.Length; i++) {
            players[i].SetupPlayer(i, acceleration, deceleration, maxSpeed, pushBackAmount, pushBackSpeed, finishLineZ);
        }
        playingPlayers = players.Length;

        isLightRed = true;
        timer = gameTime;
        timerText.text = gameTime.ToString("F1");

        UpdateRaceProgress();
    }

    private void Update() {
        if (gameStarting) {
            gameStartTimer -= Time.deltaTime;
            preGameTimerText.text = $"Game Starts In: {gameStartTimer.ToString("F1")}";
            if (gameStartTimer < 0) {
                gameStarting = false;
                gameRunning = true;
                preGameTimer.gameObject.SetActive(false);
                gameTimer.gameObject.SetActive(true);

                // Turn light green
                StartCoroutine(RotateCoroutine(180));
                isLightRed = false;

                // Start light timer
                StartCoroutine(LightController());
                
            }
        }
        else if (gameRunning) {
            UpdateRaceProgress();
            timer -= Time.deltaTime;
            timerText.text = timer.ToString("F1");
            if (timer <= 0.0f) {
                GameOver();
            }
            float lastPlaceZ = players[0].transform.position.z; // Variable for camera follow
            for (int i = 0; i < players.Length; i++) {
                if (!players[i].IsFinished) {
                    players[i].Move(isLightRed);
                    if (players[i].CheckFinish()) {
                        playingPlayers--;
                        timer -= finishTimerDecreaseAmount;
                        playerIcons[i].enabled = false;
                        if (firstFinishIndex == -1) firstFinishIndex = i;
                    }
                }

                // For camera follow
                if (players[i].transform.position.z < lastPlaceZ) {
                    lastPlaceZ = players[i].transform.position.z;
                }
            }

            // Smoothly move the camera forwards (or back) to follow last place player
            Vector3 targetPosition = followCam.transform.position - new Vector3(0f, 0f, minFollowDistance);
            targetPosition.z = Mathf.Clamp(targetPosition.z, lastPlaceZ - maxFollowDistance, lastPlaceZ - minFollowDistance);
            if (targetPosition.z > maxFollowZ) targetPosition.z = maxFollowZ;
            followCam.transform.position = Vector3.Lerp(followCam.transform.position, targetPosition, followSpeed * Time.deltaTime);

            if (lightJustSwitched) {
                reactionTimeTimer += Time.deltaTime;
                if (reactionTimeTimer > reactionTimeBuffer) {
                    reactionTimeTimer = 0;
                    lightJustSwitched = false;
                }
            }
            if (isLightRed && !lightJustSwitched) {
                foreach (RLGL_Character player in players) {
                    if (player.IsMoving) {
                        player.PushBack();
                    }
                }
            }

           if (playingPlayers <= 0) {
                GameOver();
            }
        }
    }

    private void UpdateRaceProgress() {
        for (int i = 0; i < players.Length; i++) {
            float distanceRatio = players[i].DistanceRatio();
            distanceRatio = Mathf.Clamp01(distanceRatio);
            float targetX = Mathf.Lerp(progressMinX, progressMaxX, distanceRatio);

            RectTransform rect = playerIcons[i].rectTransform;
            Vector2 newPosition = rect.anchoredPosition;
            newPosition.x = targetX;
            rect.anchoredPosition = newPosition;
        }

        // Bring their image to top by setting transform.SetAsLastSibling();
        int farthestForwardIndex = -1;
        for (int i = 0; i < playerIcons.Length; i++) {
            if (playerIcons[i].enabled == false) continue;
            float distanceRatio = players[i].DistanceRatio();
            if (farthestForwardIndex == -1 || distanceRatio > players[farthestForwardIndex].DistanceRatio()) {
                farthestForwardIndex = i;
            }
        }
        if (farthestForwardIndex != -1) playerIcons[farthestForwardIndex].transform.SetAsLastSibling();
    }

    private void GameOver() {
        if (!gameRunning) return;
        gameRunning = false;
        runLightCoroutine = false;

        // COMMENT THIS OUT IF TESTING LOCALLY
        
        int[] moveData = new int[GameManager.instance.numPlayers];
        int spacesToMove = GameManager.instance.diceRoll;

        for (int i = 0; i < players.Length; i++) {
            if (!players[i].IsFinished) players[i].DropModel();
            moveData[i] = players[i].IsFinished ? spacesToMove : 0;
        }
        if (firstFinishIndex != -1) moveData[firstFinishIndex] += firstFinishBonus;

        GameManager.instance.MoveData(moveData);
        
        // END COMMENT REGION IF TESTING LOCALLY

        // back to board
        StartCoroutine(ReturnToBoardCoroutine());
    }

    private IEnumerator ReturnToBoardCoroutine() {
        yield return new WaitForSeconds(4);

        SceneManager.LoadScene(1);
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
