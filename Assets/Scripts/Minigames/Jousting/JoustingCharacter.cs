using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoustingCharacter : MonoBehaviour {
    private int playerIndex;
    private float speed;

    private bool bButtonAPressed = false;

    private Vector2 direction;

    private JoustingManager manager;

    public bool IsButtonAPressed { get { return bButtonAPressed; } }


    private void Start() {
        manager = GameObject.Find("JoustingManager").GetComponent<JoustingManager>();
        manager.SetupPlayer(this);
    }

    public void SetupPlayer(int playerIndex, float speed) {
        this.playerIndex = playerIndex;
        this.speed = speed;
    }

    public void OnButtonA(bool value) {
        bButtonAPressed = value;
    }

    public void OnInputMove(Vector2 direction) {
        this.direction = direction;
    }

    private void Update() {
        Vector3 newPos = this.transform.localPosition;
        if (direction.y > 0) {
            // move forward
            newPos.z += speed * Time.deltaTime;
        }
        if (direction.y < 0) {
            // move back
            newPos.z -= speed * Time.deltaTime;
        }
        if (direction.x > 0) {
            // Turn right
        }
        if (direction.x < 0) {
            // Turn left
        }
        
        /*
        newPos.x += direction.x * Time.deltaTime * speed;
        newPos.z += direction.y * Time.deltaTime * speed;*/
        this.transform.localPosition = newPos;
    }

}
