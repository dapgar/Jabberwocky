using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrownKeepCharacter : MonoBehaviour {

    private bool bButtonAPressed = false;

    private Vector2 direction;

    private CrownKeepManager manager;

    private bool canGetHit;

    /* Movement */
    [SerializeField]
    private float acceleration = 12f;
    [SerializeField]
    private float deceleration = 12f;
    [SerializeField]
    private float maxSpeed = 8f;
    private float currentSpeed = 0f;
    [SerializeField]
    private float turnSpeed = 90f;

    public bool IsButtonAPressed { get { return bButtonAPressed; } }

    private void Start() {
        manager = GameObject.Find("CrownKeepManager").GetComponent<CrownKeepManager>();
        manager.GetPlayers(this);
        canGetHit = true;
    }

    public void OnButtonA(bool value) {
        bButtonAPressed = value;
    }

    public void OnInputMove(Vector2 direction) {
        Debug.Log("dir in: " + direction);
        if (direction.x < 0) direction.x = -1;
        if (direction.x > 0) direction.x = 1;
        if (direction.y < 0) direction.y = -1;
        if (direction.y > 0) direction.y = 1;
        this.direction = direction;
    }

    private void Update() {
        MovePlayer();
    }

    private void MovePlayer() {
        Debug.Log("Dir: " + direction);
        //gameObject.transform.Rotate(0f, direction.x * turnSpeed * Time.deltaTime, 0f);
        transform.forward += transform.right * direction.x * turnSpeed * Time.deltaTime;

        if (direction.y > 0 || direction.y < 0) {
            // Accelerate
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
        }
        else {
            // Deccelerate
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
        }
        gameObject.transform.Translate(0f, 0f, direction.y * currentSpeed * Time.deltaTime);
        Debug.Log("Dir 2: " + direction);
    }
}
