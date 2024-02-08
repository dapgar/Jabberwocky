using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using TMPro;

public class RLGL_Character : MonoBehaviour {
    [SerializeField]
    private Camera cam;

    private bool bIsMoving = false;
    private bool bIsFinished = false;
    private bool bIsAlive = true;
    private int playerIndex;
    private Key key;
    public bool IsAlive { get { return bIsAlive; } }
    public bool IsFinished { get { return bIsFinished; } }
    public bool IsMoving { get { return bIsMoving; } }
    public int PlayerIndex { get { return playerIndex; } }

    //private float speed;

    [Header("Win Stuff")]
    [SerializeField]
    private float winMoveSpeed = 3f;
    [SerializeField]
    private float winZMoveOffset = 1f;
    [SerializeField]
    private float winCamZOffset = 6f;
    [SerializeField]
    private float winCamRotateSpeed = 1f;

    [Header("Death Stuff")]
    [SerializeField]
    private Transform model;

    [SerializeField]
    private float deathRotateSpeed = 5f;
    private float deathYOffset = 0.35f;

    private float acceleration = 2f;
    private float deceleration = 4f;
    private float maxSpeed = 5f;
    private float currentSpeed = 0f;

    void SetMovementKey() {
        if (playerIndex == 0) {
            key = Key.Q;
        }
        else if (playerIndex == 1) {
            key = Key.R;
        }
        else if (playerIndex == 2) {
            key = Key.U;
        }
        else {
            key = Key.P;
        }
    }

    public void Move() {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return; // no keyboard

        if (keyboard[key].isPressed) {
            Accelerate();
            // For static movement without acceleration/deceleration, uncomment below
            //gameObject.transform.Translate(new Vector3(0f, 0f, 1) * maxSpeed * Time.deltaTime);
            bIsMoving = true;
        }
        else if (bIsMoving) {
            Decelerate();
        }

        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

        // For static movement without acceleration/deceleration, uncomment below
        // There is no "key released" so it just checks if they're NOT pressing key but were previously moving, if so sets moving to false
        /*if (!keyboard[key].isPressed && bIsMoving) {
            bIsMoving = false;
        }*/
    }

    void Accelerate() {
        currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
    }

    void Decelerate() {
        currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);

        if (currentSpeed == 0f) {
            bIsMoving = false;
        }
    }

    public bool CheckFinish(float finishLineZ) {
        if (gameObject.transform.position.z >= finishLineZ) {
            Debug.Log($"Player {playerIndex + 1} finished");
            bIsFinished = true;
            bIsMoving = false;

            Quaternion targetRotation = Quaternion.Euler(0f, 180f, 0f);
            Vector3 targetPosition = gameObject.transform.position + new Vector3(0f, 0f, winZMoveOffset);
            Vector3 targetCamPosition = cam.transform.position + new Vector3(0f, 0f, winCamZOffset);
            StartCoroutine(MoveOnWin(gameObject.transform, targetPosition, winMoveSpeed));
            StartCoroutine(MoveCamOnWin(cam.transform, targetCamPosition, targetRotation, winCamRotateSpeed));

            return true;
        }
        return false;
    }

    /// <summary>
    /// Sets up split screen cameras for each player.
    /// </summary>
    public void SetupPlayer(int playerIndex, int totalPlayers, float acceleration, float deceleration, float maxSpeed) {
        this.playerIndex = playerIndex;
        this.acceleration = acceleration;
        this.deceleration = deceleration;
        this.maxSpeed = maxSpeed;

        SetMovementKey();

        // TODO: Only setup for 4 players ATM, need to support 2-4 after MVI
        if (totalPlayers == 4) {
            if (playerIndex == 0) {
                cam.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
            }
            else if (playerIndex == 1) {
                cam.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
            }
            else if (playerIndex == 2) {
                cam.rect = new Rect(0, 0f, 0.5f, 0.5f);
            }
            else if (playerIndex == 3) {
                cam.rect = new Rect(0.5f, 0, 0.5f, 0.5f);
            }
        }
        // Theoretical code for 2-3 player split screen, not tested yet
        else if (totalPlayers == 3) {
            if (playerIndex == 0) {
                cam.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
            }
            else if (playerIndex == 1) {
                cam.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
            }
            else {
                cam.rect = new Rect(0, 0f, 1f, 0.5f);
            }
        }
        else {
            if (playerIndex == 0) {
                cam.rect = new Rect(0, 0f, 0.5f, 1f);
            }
            else if (playerIndex == 1) {
                cam.rect = new Rect(0.5f, 0f, 0.5f, 1f);
            }
        }
    }

    public void Kill() {
        // Debug.Log($"Player {playerIndex + 1} died");
        bIsAlive = false;
        
        // Drop model
        if (model != null) {
            Quaternion targetRotation = Quaternion.Euler(-90f, 0f, 0f);
            StartCoroutine(RotateOverTime(model, targetRotation, deathRotateSpeed));

            Vector3 targetPosition = model.position + new Vector3(0f, deathYOffset, 0f);
            StartCoroutine(MoveYPositionOverTime(model, targetPosition, deathRotateSpeed));
        }

        // Point camera down facing model
        Quaternion cameraStartRotation = cam.transform.rotation;
        Quaternion cameraTargetRotation = Quaternion.Euler(30f, cameraStartRotation.eulerAngles.y, cameraStartRotation.eulerAngles.z);
        StartCoroutine(MoveCameraOverTime(cam.transform, cameraTargetRotation, deathRotateSpeed));

    }

    private IEnumerator RotateOverTime(Transform targetTransform, Quaternion targetRotation, float speed) {
        float t = 0f;
        Quaternion startRotation = targetTransform.rotation;

        while (t < 1) {
            t += Time.deltaTime * speed;
            targetTransform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null;
        }
    }

    private IEnumerator MoveYPositionOverTime(Transform targetTransform, Vector3 targetPosition, float speed) {
        float t = 0f;
        Vector3 startPosition = targetTransform.position;

        while (t < 1) {
            t += Time.deltaTime * speed;
            targetTransform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }
    }

    private IEnumerator MoveCameraOverTime(Transform targetTransform, Quaternion targetRotation, float speed) {
        float t = 0f;
        Quaternion startRotation = targetTransform.rotation;

        while (t < 1) {
            t += Time.deltaTime * speed;
            targetTransform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null;
        }
    }

    private IEnumerator MoveOnWin(Transform targetTransform, Vector3 targetPosition, float speed) {
        float t = 0f;
        Vector3 startPosition = targetTransform.position;

        while (t < 1) {
            t += Time.deltaTime * speed;
            targetTransform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;

        }
    }

    private IEnumerator MoveCamOnWin(Transform targetTransform, Vector3 targetPosition, Quaternion targetRotation, float speed) {
        float t = 0f;
        Quaternion startRotation = targetTransform.rotation;
        Vector3 startPosition = targetTransform.position;

        while (t < 1) {
            t += Time.deltaTime * speed;
            targetTransform.position = Vector3.Lerp(startPosition, targetPosition, t);
            targetTransform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null;

        }
    }


}
