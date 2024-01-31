using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DiceRollManager : MonoBehaviour
{
    public float minForce = 5f;
    public float maxForce = 10f;
    public GameObject dicePF;
    public bool isRolling = false;

    private Rigidbody rb;
    private int resultNumber;
    private GameObject dice;

    public void RollDice()
    {
        isRolling = true;

        dice = Instantiate(dicePF, Camera.main.transform.position + Camera.main.transform.forward, Quaternion.identity);
        rb = dice.GetComponent<Rigidbody>();

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        float force = Random.Range(minForce, maxForce);

        // Calculate the direction from the camera's forward direction
        Vector3 direction = Camera.main.transform.forward + new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));

        rb.AddForce(direction * force, ForceMode.Impulse);

        // Applies a random torque
        Vector3 randomTorque = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));
        rb.AddTorque(randomTorque, ForceMode.Impulse);

        // Destroys Dice
        GameObject.Destroy(dice, 4.1f);
    }

    public int DetectResult()
    {
        isRolling = false;

        // Raycast to detect the face of the dice that is facing upwards
        RaycastHit hit;
        if (Physics.Raycast(dice.transform.position, Vector3.up, out hit))
        {
            Debug.DrawRay(dice.transform.position, Vector3.up, Color.green);

            // Assuming each face of the dice has a unique tag or identifier
            string resultFace = hit.collider.gameObject.tag;

            // Now you can use the resultFace to determine the outcome of the roll
            // For example, if you have a standard six-sided dice, you can assign a number (1 to 6) to each face
            resultNumber = 0;

            switch (resultFace)
            {
                case "Face1":
                    resultNumber = 1;
                    break;
                case "Face2":
                    resultNumber = 2;
                    break;
                case "Face3":
                    resultNumber = 3;
                    break;
                case "Face4":
                    resultNumber = 4;
                    break;
                case "Face5":
                    resultNumber = 5;
                    break;
                case "Face6":
                    resultNumber = 6;
                    break;
                default:
                    Debug.LogError("Unknown face detected!");
                    break;
            }

            Debug.Log("Result: " + resultNumber);
            // Now you can use the resultNumber to update your game logic accordingly
            // For example, if you're playing a board game, you can move the player by resultNumber spaces
        }
        else
        {
            Debug.LogError("No face detected!");
            resultNumber = 0;
        }

        return resultNumber;
    }
}
