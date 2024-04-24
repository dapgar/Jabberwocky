using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrownKeepCharacter : MonoBehaviour {
    private Vector2 direction;

    private CrownKeepManager manager;

    /* Movement */
    [SerializeField]
    private float maxSpeed = 7f;
    private Vector3 currentVelocity;
    [SerializeField]
    private float turnSpeed = 250f;
    [SerializeField]
    private Rigidbody rb;
    private Quaternion rotation;

    /* Crown Stuff */
    private float crownTime;
    private float currentCrownTime;
    private bool bHasCrown;
    [SerializeField]
    private float crownStealBuffer = 0.15f;


    public float CrownTime { get { return crownTime; } }


    private void Start() {
        manager = GameObject.Find("CrownKeepManager").GetComponent<CrownKeepManager>();
        manager.GetPlayers(this);
    }

    public void OnInputMove(Vector2 direction) {
        if (direction.x < 0) direction.x = -1;
        if (direction.x > 0) direction.x = 1;
        if (direction.y < 0) direction.y = -1;
        if (direction.y > 0) direction.y = 1;
        this.direction = direction;
    }

    private void Update() {
        if (manager.PlayersCanMove) MovePlayer();
        if (bHasCrown) {
            currentCrownTime += Time.deltaTime;
            crownTime += Time.deltaTime;
        }
    }

    private void MovePlayer() {
        currentVelocity.Set(direction.x, 0f, direction.y);
        currentVelocity.Normalize();

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, currentVelocity, turnSpeed * Time.deltaTime, 0f);
        rotation = Quaternion.LookRotation(desiredForward);

        rb.MovePosition(rb.position + currentVelocity * maxSpeed * Time.deltaTime);
        rb.MoveRotation(rotation);
    }

    private void OnTriggerEnter(Collider other) {
        manager.OnCrownTouched(this);
    }

    public bool CanGetStolen() {
        return currentCrownTime > crownStealBuffer;
    }

    public void CrownStolen() {
        bHasCrown = false;
        currentCrownTime = 0f;
        Debug.Log("LOST CROWN");
    }

    public void GotCrown() {
        bHasCrown = true;
        Debug.Log("GOT CROWN");
    }
}
