using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDemo : MonoBehaviour, IInputReceiver
{
    public void OnButton()
    {
        transform.Translate(new Vector3(0, 10 * Time.deltaTime, 0));
    }
}
