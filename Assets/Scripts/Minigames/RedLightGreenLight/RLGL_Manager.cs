using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RLGL_Manager : MonoBehaviour {
    [SerializeField]
    private float speed;

    [SerializeField]
    private RLGL_Character[] players;

    [SerializeField]
    private Transform lightIndicator;
    [SerializeField]
    private float rotationTime = 0.75f;

    private bool[] movingPlayers;
    private bool isLightRed = true;

    [SerializeField]
    private float maxRedLightDuration = 4f;
    [SerializeField]
    private float minRedLightDuration = 1.5f;

    [SerializeField]
    private float maxGreenLightDuration = 5f;
    [SerializeField]
    private float minGreenLightDuration = 2f;

    [SerializeField]
    private float finishLineZ;

    /* Higher reaction time buffer makes game easier */
    [SerializeField]
    private float reactionTimeBuffer = 0.75f;
    private bool lightJustSwitched = false;
    private float reactionTimeTimer = 0f;

    private bool runLightCoroutine = true;  // For cleanup, set this to false to stop looping coroutine

    /*
     * Could be better with a RLGL_Character class with the following:
     * bool for moving
     * bool for alive
     * bool for isFinished
     * 
     * that way we can do:
     * players[index].moving
     * players[index].finished or .dead --> Don't take input actions
     * 
     * Would need own prefabs for characters for this game
     * prefabs would need a RLGL_Character script on them
     */

    private void Start() {
        movingPlayers = new bool[players.Length];
        for (int i = 0; i < movingPlayers.Length; i++) {
            movingPlayers[i] = false;
        }

        StartCoroutine(LightController());

        for (int i = 0; i < players.Length; i++) {
            players[i].SetupCamera(i, players.Length);
        }
    }

    private void Update() {
        MovePlayer(0, Key.Q);
        MovePlayer(1, Key.R);
        MovePlayer(2, Key.U);
        MovePlayer(3, Key.P);

        if (lightJustSwitched) {
            reactionTimeTimer += Time.deltaTime;
            if (reactionTimeTimer > reactionTimeBuffer) {
                reactionTimeTimer = 0;
                lightJustSwitched = false;
            }
        }   // May be better for "else if", however that may save them 1 frame longer
        if (isLightRed && !lightJustSwitched) {
            for (int i = 0; i < movingPlayers.Length; i++) {
                if (movingPlayers[i]) {
                    // Kill that player
                    Debug.Log($"Player {i + 1} died");
                }
            }
        }
    }


    private void MovePlayer(int index, Key key) {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return; // no keyboard

        if (keyboard[key].isPressed) {
            players[index].transform.Translate(new Vector3(0f, 0f, 1) * speed * Time.deltaTime);
            movingPlayers[index] = true;
        }

        // There is no "key released" so it just checks if they're NOT pressing key but were previously moving, if so sets moving to false
        if (!keyboard[key].isPressed && movingPlayers[index]) {
            movingPlayers[index] = false;
        }

        if (players[index].transform.position.z >= finishLineZ) {
            // TODO: Actual win & safety for that player, disable movement & face them towards camera?
            Debug.Log($"Player {index + 1} finished");
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
