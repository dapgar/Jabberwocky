using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RLGL_Movement : MonoBehaviour {
    [SerializeField]
    private float speed;

    [SerializeField]
    private GameObject[] players;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            // P1 move forward
            players[0].transform.position = new Vector3(0f, 0f, transform.position.z - speed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            // P2 move forward
        }
        if (Input.GetKeyDown(KeyCode.U)) {
            // P3 move forward
        }
        if (Input.GetKeyDown(KeyCode.P)) {
            // P4 move forward
        }
    }
}
