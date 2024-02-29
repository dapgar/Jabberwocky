using UnityEngine;
using System.Collections;

public class RLGL_Character : MonoBehaviour {
    [Header("Win Stuff")]
    [SerializeField]
    private float winMoveSpeed = 3f;
    [SerializeField]
    private float winZMoveOffset = 1f;

    [Header("Model Stuff")]
    [SerializeField]
    private Transform model;
    [SerializeField]
    private float dropModelRotateSpeed = 5f;
    private float dropModelOffsetY = 0.35f;

    private bool bIsMoving = false;
    private bool bIsFinished = false;
    private int playerIndex;

    private float bobbingAmount = 2f;
    private float bobbingSpeed = 1f;
    private float bobbingResetSpeed = 3f;

    private float acceleration = 2f;
    private float deceleration = 4f;
    private float maxSpeed = 5f;
    private float currentSpeed = 0f;

    private bool bCanGetPushedBack = true;
    private float pushBackDelay = 0.8f;
    private float pushBackAmount;
    private float pushBackSpeed;

    private float finishLineZ;
    private float totalDistance;
    private float startingZ;

    private bool bButtonPressed = false;
    public RLGL_Manager manager;
    public bool IsFinished { get { return bIsFinished; } }
    public bool IsMoving { get { return bIsMoving; } }
    public int PlayerIndex { get { return playerIndex; } }

    public void OnButton(bool value) {
        bButtonPressed = value;
    }

    private void Start() {
        manager = GameObject.Find("RLGL_Manager").GetComponent<RLGL_Manager>();
        manager.SetupPLayers(this);
    }

    /// <summary>
    /// Sets up variables for the player.
    /// </summary>
    public void SetupPlayer(int playerIndex, float acceleration, float deceleration, float maxSpeed, float pushBackAmount, float pushBackSpeed, float finishLineZ) {
        totalDistance = Mathf.Abs(transform.position.z - finishLineZ);
        startingZ = transform.position.z;

        this.playerIndex = playerIndex;
        this.acceleration = acceleration;
        this.deceleration = deceleration;
        this.maxSpeed = maxSpeed;
        this.pushBackAmount = pushBackAmount;
        this.pushBackSpeed = pushBackSpeed;
        this.finishLineZ = finishLineZ;

       //SetMovementKey();

        // Split Screen Setup:  Splitscreen 
        // TODO: Only setup for 4 players ATM, need to support 2-4 after MVI
        /*if (totalPlayers == 4) {
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
        }*/
    }

    /*void SetMovementKey() {
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
    }*/

    public void Move(bool bIsLightRed) {
        //if (bIsLightRed && !bCanGetPushedBack) return;  // Light is red and currently mid push-back, disable movement
        /*Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;*/ // if no keyboard
        Debug.Log("Move " + bButtonPressed);
        if (bButtonPressed) {
            Accelerate();
            bIsMoving = true;
        }
        else if (bIsMoving) {
            Decelerate();
        }

        // Bob character left & right
        /*if (bIsMoving && model != null) {
            float bobbingAngle = Mathf.Lerp(-bobbingAmount, bobbingAmount, Mathf.PingPong(Time.time * bobbingSpeed, 1f));
            model.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, bobbingAngle);
        }
        else {
            model.transform.rotation = Quaternion.Lerp(model.transform.rotation, Quaternion.Euler(0f, 0f, 0f), Time.deltaTime * bobbingResetSpeed);
        }*/

        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
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

    public bool CheckFinish() {
        if (gameObject.transform.position.z >= finishLineZ) {
            bIsFinished = true;
            bIsMoving = false;

            StartCoroutine(MoveAndRotateOnFinish());
            return true;
        }
        return false;
    }

    public void DropModel() {
        if (model != null) {
            Quaternion targetRotation = Quaternion.Euler(-90f, 0f, 0f);
            StartCoroutine(RotateOverTime(model, targetRotation, dropModelRotateSpeed));

            Vector3 targetPosition = model.position + new Vector3(0f, dropModelOffsetY, 0f);
            StartCoroutine(MoveYPositionOverTime(model, targetPosition, dropModelRotateSpeed));
        }
    }

    /// <summary>
    /// Pushes player back by specified amount
    /// </summary>
    public void PushBack() {
        if (!bCanGetPushedBack) return;
        //bIsMoving = false;
        StartCoroutine(PushBackTimerCoroutine());
        StartCoroutine(PushBackCoroutine());
    }

    private IEnumerator PushBackCoroutine() {
        Vector3 targetPosition = transform.position + new Vector3(0f, 0f, -pushBackAmount);
        targetPosition.z = targetPosition.z < startingZ ? targetPosition.z = startingZ : targetPosition.z;  // Ensure they don't get pushed back beyond start
        Vector3 startPosition = transform.position;
        float t = 0f;
        while (t < 1) {
            t += Time.deltaTime * pushBackSpeed;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }
    }

    private IEnumerator PushBackTimerCoroutine() {
        bCanGetPushedBack = false;
        yield return new WaitForSeconds(pushBackDelay);
        bCanGetPushedBack = true;
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

    private IEnumerator MoveAndRotateOnFinish() {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + new Vector3(0f, 0f, winZMoveOffset);
        
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0f, 180f, 0f);
        
        float t = 0f;

        while (t < 1) {
            t += Time.deltaTime * winMoveSpeed;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null;

        }
    }

    public float DistanceRatio() {
        return transform.position.z / totalDistance;
    }
}
