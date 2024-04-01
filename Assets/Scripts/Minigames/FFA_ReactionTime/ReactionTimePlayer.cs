using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionTimePlayer : MonoBehaviour
{
    public int playerIndex;
    private Animator animator;

    ReactionTime manager;

    // Start is called before the first frame update
    private void Start()
    {
        playerIndex = GetComponent<PlayerInputHandler>().GetIndex();
        Debug.Log(playerIndex);

        manager = FindAnyObjectByType<ReactionTime>();
        manager.SetupPlayer(this);
    }

    public void Setup()
    {
        Transform playerTransform = transform.Find($"Player{playerIndex + 1}(Clone)");
        animator = playerTransform.GetComponent<Animator>();
    }

    // Update is called once per frame
    public void OnButton(bool value)
    {
        // PRESS
        if (value)
        {
            manager.HandlePlayerInput(this);
        }
    }

    public void Backflip()
    {
        if (animator) animator.SetTrigger("Backflip");
    }

    public void Die()
    {
        if (animator) animator.SetTrigger("Dead");
    }
}
