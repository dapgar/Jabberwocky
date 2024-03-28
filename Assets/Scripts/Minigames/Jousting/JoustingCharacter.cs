using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoustingCharacter : MonoBehaviour {
    private int playerIndex;
    private float speed;
    private float turnSpeed;

    private bool bButtonAPressed = false;

    private Vector2 direction;

    private JoustingManager manager;

    private int hp = 2;

    private bool canGetHit;

    /* Movement */
    [SerializeField]
    private float acceleration = 6f;
    [SerializeField]
    private float deceleration = 6f;
    [SerializeField]
    private float maxSpeed = 8f;
    private float currentSpeed = 0f;

    public bool IsButtonAPressed { get { return bButtonAPressed; } }

    private void Start() {
        manager = GameObject.Find("JoustingManager").GetComponent<JoustingManager>();
        manager.SetupPlayer(this);
        canGetHit = true;
    }

    public void SetupPlayer(int playerIndex, float speed, float turnSpeed) {
        this.playerIndex = playerIndex;
        this.speed = speed;
        this.turnSpeed = turnSpeed;
    }

    public void OnButtonA(bool value) {
        bButtonAPressed = value;
    }

    public void OnInputMove(Vector2 direction) {
        if (direction.x < 0) direction.x = -1;
        if (direction.x > 0) direction.x = 1;
        if (direction.y < 0) direction.y = -1;
        if (direction.y > 0) direction.y = 1;
        this.direction = direction;
    }

    private void Update() {
        MovePlayer();

        if (hp <= 0) {
            Debug.Log($"Player {playerIndex + 1} died");
        }
    }

    private void MovePlayer() {
        // Todo: add acceleration / decceleration
        gameObject.transform.Rotate(0f, direction.x * turnSpeed * Time.deltaTime, 0f);

        if (direction.y > 0) {
            // Accelerate
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
        }
        else {
            // Deccelerate
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
        }
        gameObject.transform.Translate(0f, 0f, direction.y * currentSpeed * Time.deltaTime);

    }

    private void OnTriggerEnter(Collider other) {
        if (!canGetHit) return;
        Vector3 direction = other.transform.position - transform.position;
        float dotProduct = Vector3.Dot(transform.forward, direction.normalized);
        if (dotProduct > 0) {
            Debug.Log("Trigger Hit from the front");
        }
        else {
            Debug.Log("Trigger Hit from the back");
            hp--;
            StartCoroutine(HitCoroutine());
        }
    }

    IEnumerator HitCoroutine() {
        canGetHit = false;
        yield return new WaitForSeconds(1.5f);
        canGetHit = true;
    }
}
