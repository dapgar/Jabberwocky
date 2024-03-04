using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionTimePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void OnButton(bool value)
    {
        // PRESS
        if (value)
        {
            GameObject.FindAnyObjectByType<ReactionTime>()?.HandlePlayerInput(GetComponent<PlayerInputHandler>().GetIndex());
        }
    }
}
