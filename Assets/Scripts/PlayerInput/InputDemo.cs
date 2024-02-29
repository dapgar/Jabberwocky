using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDemo : MonoBehaviour
{
    [SerializeField]
    private bool onHold = true;
    bool buttonPressed = false;

    private Vector3 moveDirection = Vector3.zero;
    private Vector2 inputVector = Vector2.zero;

    private void Update()
    {
        moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= 5f; // movespeed

        transform.Translate(moveDirection * Time.deltaTime);

        if (buttonPressed)
        {
            transform.Translate(new Vector3(0, 2 * Time.deltaTime, 0));
        }
    }

    public void OnButton(bool value)
    {
        if (onHold)
        {
            buttonPressed = value;
        }
        else if (value)
        {
            transform.Translate(new Vector3(0, 10 * Time.deltaTime, 0));
        }
    }

    public void OnInputMove(Vector2 direction)
    {
        inputVector = direction;
    }
}
