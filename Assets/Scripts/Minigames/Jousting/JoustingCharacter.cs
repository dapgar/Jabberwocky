//using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JoustingCharacter : MonoBehaviour {
    [SerializeField]
    private GameEvent onDie;

    [SerializeField]
    private Animator animator;

    public int playerIndex;
    private float speed;
    private float turnSpeed;

    private bool bButtonAPressed = false;

    private Vector2 direction;

    private JoustingManager manager;

    private int hp = 3;

    private bool canGetHit;

    [SerializeField]
    GameObject joustObj;

    /* Movement */
    [SerializeField]
    private float acceleration = 12f;
    [SerializeField]
    private float deceleration = 12f;
    [SerializeField]
    private float maxSpeed = 8f;
    private float currentSpeed = 0f;

    private float maxTurnSpeed = 2.4f;
    private float minTurnSpeed = 1f;

    private bool won;

    [SerializeField]
    private GameObject[] crowns = new GameObject[3];

    public bool IsButtonAPressed { get { return bButtonAPressed; } }

    private void Start() {
        manager = GameObject.Find("JoustingManager").GetComponent<JoustingManager>();
        manager.SetupPlayer(this);
        canGetHit = true;
    }

    public void SetupPlayer(int playerIndex, float speed, float turnSpeed) {
        this.playerIndex = playerIndex;
        this.maxSpeed = speed;
        this.turnSpeed = turnSpeed;

        Transform playerTransform = transform.Find($"Player{playerIndex + 1}(Clone)");
        animator = playerTransform.GetComponent<Animator>();
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
        if (won)
        {
            return;
        }

        MovePlayer();

        //if (hp <= 0) {
        //    Debug.Log($"Player {playerIndex + 1} died");
        //}
    }

    private void MovePlayer() {
        // Todo: add acceleration / decceleration
        //gameObject.transform.Rotate(0f, direction.x * turnSpeed * Time.deltaTime, 0f);

        //transform.forward += transform.forward * -direction.y * currentSpeed * Time.deltaTime * 2;

        //currentSpeed += (20 * direction.y * Time.deltaTime) / 10 + -4 * Time.deltaTime;

        float distToCenter = Mathf.Sqrt(Mathf.Pow(transform.position.x, 2) + Mathf.Pow(transform.position.z, 2));
        if (distToCenter >= 12.0f)
        {
            transform.forward = new Vector3(-transform.position.x, 0, -transform.position.z);
            gameObject.transform.localPosition += transform.forward * currentSpeed * Time.deltaTime;
            currentSpeed = 2;
        }

        if (direction.y > 0) {
            // Accelerate
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
        }
        else if (direction.y < 0)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * 2 * Time.deltaTime);
        }
        else {
            // Deccelerate
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
        }
        

        if (currentSpeed < 0f)
        {
            currentSpeed = 0f;
        }

        // harder to turn at faster speeds
        turnSpeed = minTurnSpeed + (1 - Mathf.Pow(currentSpeed / maxSpeed, 1)) * maxTurnSpeed;
        // adjust joust rot base on speed
        joustObj.transform.localEulerAngles = new Vector3((1 - Mathf.Clamp(currentSpeed / (maxSpeed * .6f), 0, 1)) * -80, joustObj.transform.localEulerAngles.y, joustObj.transform.localEulerAngles.z);

        // add rotation
        transform.forward += transform.right * direction.x * turnSpeed * Time.deltaTime;
        
        // move forward
        gameObject.transform.localPosition += transform.forward * currentSpeed * Time.deltaTime;

        
        //transform.Translate(transform.forward * currentSpeed * Time.deltaTime);
        //Debug.Log(currentSpeed);
    }

    private void OnTriggerEnter(Collider other) {
        if (!canGetHit) return;
        Vector3 direction = other.transform.position - transform.position;
        float dotProduct = Vector3.Dot(transform.forward, direction.normalized);
        //if (dotProduct > 0) {
        //    Debug.Log("Trigger Hit from the front");
        //}
        //else {
        //    Debug.Log("Trigger Hit from the back");
        //    hp--;
        //    StartCoroutine(HitCoroutine());
        //}
    }

    public void GetHit()
    {
        hp--;
        //crowns[hp].transform.parent = null;
        crowns[hp].AddComponent<Rigidbody>();
        crowns[hp].GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-25, 25) * 4, Random.Range(-30, 30) + 180, Random.Range(-25, 25) * 4));
        crowns[hp].GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-90, 90), Random.Range(-90, 90), Random.Range(-90, 90)));

        StartCoroutine(HitCoroutine());
        if (hp <= 0)
        {
            onDie.Invoke();
            Destroy(this.gameObject);
        }
    }

    public void ShieldHit()
    {
        currentSpeed = 0;
        Debug.Log("Shield has been hit.");
    }

    IEnumerator HitCoroutine() {
        canGetHit = false;
        if (animator) animator.SetTrigger("Dead");
        yield return new WaitForSeconds(1.5f);
        if (animator) animator.SetTrigger("Dead");
        canGetHit = true;
    }

    public void Win()
    {
        if (animator) animator.SetTrigger("Backflip");
        won = true;
    }
}
