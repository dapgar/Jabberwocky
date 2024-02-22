using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputMan : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnPlayerJoined(PlayerInput player)
    {
        Debug.Log("Player Joined: " + player);
    }

    void OnPlayerLeft(PlayerInput player)
    {
        Debug.Log("Player Left: " + player);
    }
}
