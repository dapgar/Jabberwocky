using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LP_Player : MonoBehaviour
{
    private bool buttonPressed = false;
    //private float jumpPower = 0.0f;

    private Vector3 moveDirection = Vector3.zero;
    private Vector2 inputVector = Vector2.zero;

    private Vector2 testVector = Vector2.zero;

    private void Start()
    {
        testVector = new Vector2(Random.Range(-1,2), Random.Range(-1,2));
        testVector = testVector.normalized;
        Debug.Log(testVector);
    }

    private void Update()
    {
        // --- DPAD ---
        //moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
        //moveDirection = transform.TransformDirection(moveDirection);
        //moveDirection *= 5f; // movespeed

        //transform.Translate(moveDirection * Time.deltaTime);



        // --- BUTTON ---
        //// PRESS (nothing)

        //// RELEASE (nothing)

        // HOLD
        if (buttonPressed)
        {
            transform.Translate(new Vector3(0, 2 * Time.deltaTime, 0));
        }

        if (inputVector == testVector)
        {
            testVector = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));
            testVector = testVector.normalized;
            Debug.Log(testVector);
        }

        //// HOLD AND RELEASE
        //if (buttonPressed)
        //{
        //    jumpPower += 1.0f;
        //}
    }

    public void OnButton(bool value)
    {
        //// PRESS
        //if (value)
        //{
        //    transform.Translate(new Vector3(0, 10 * Time.deltaTime, 0));
        //}

        //// RELEASE
        //if (!value)
        //{
        //    transform.Translate(new Vector3(0, 10 * Time.deltaTime, 0));
        //}

        // HOLD
        buttonPressed = value;

        //// HOLD AND RELEASE
        //buttonPressed = value;
        //if (buttonPressed)
        //{
        //    // ???
        //}
        //else
        //{
        //    transform.Translate(new Vector3(0, jumpPower * Time.deltaTime, 0));
        //    jumpPower = 0.0f;
        //}
    }

    public void OnInputMove(Vector2 direction)
    {
        inputVector = direction;
        if (inputVector.x != 0)
        {

        }
        else if (inputVector.y != 0)
        {

        }
    }
}
