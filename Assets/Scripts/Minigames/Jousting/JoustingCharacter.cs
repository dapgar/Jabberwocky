using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class JoustingCharacter : MonoBehaviour {
    private int playerIndex;
    private float speed;
    private float turnSpeed;

    private bool bButtonAPressed = false;

    private Vector2 direction;

    private JoustingManager manager;

    public bool IsButtonAPressed { get { return bButtonAPressed; } }


    private void Start() {
        manager = GameObject.Find("JoustingManager").GetComponent<JoustingManager>();
        manager.SetupPlayer(this);
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
    }

    private void MovePlayer() {
        gameObject.transform.Rotate(0f, direction.x * turnSpeed * Time.deltaTime, 0f);
        gameObject.transform.Translate(0f, 0f, direction.y * speed * Time.deltaTime);
    }
}
