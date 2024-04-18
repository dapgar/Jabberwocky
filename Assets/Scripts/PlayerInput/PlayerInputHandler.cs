using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerConfiguration playerConfig;

    public int GetIndex()
    {
        return playerConfig.PlayerIndex;
    }

    private GameObject playerChar;

    private GameObject playerHat;

    private PlayerControls controls;

    public bool moveWhilePerformed;

    public BoolEvent onButtonX;
    public BoolEvent onButtonY;
    public BoolEvent onButtonA;
    public BoolEvent onButtonB;
    public BoolEvent onTriggerL;
    public BoolEvent onTriggerR;
    public BoolEvent onSelect;
    public BoolEvent onStart;
    public BoolEvent onUp;
    public BoolEvent onDown;
    public BoolEvent onLeft;
    public BoolEvent onRight;
    public Vector2Event onDPad;

    // Start is called before the first frame update
    public void Awake()
    {
        controls = new PlayerControls();
    }

    public void InitializePlayer(PlayerConfiguration pC)
    {
        playerConfig = pC;
        
        playerChar = Instantiate(pC.PlayerChar, transform.position, transform.rotation, gameObject.transform);
        playerChar.name = $"Player{pC.PlayerIndex + 1}(Clone)";
        if (pC.PlayerHat != null)
        {
            playerHat = Instantiate(pC.PlayerHat, transform.position, transform.rotation, playerChar.transform);
        }
        playerConfig.Input.onActionTriggered += Input_onActionTriggered;
    }

    private void Input_onActionTriggered(CallbackContext context)
    {
        if (context.performed)
        {
            if (moveWhilePerformed && context.action.name == controls.Minigame.Move.name)
            {
                onDPad?.Invoke(context.ReadValue<Vector2>());
            }
            return;
        }

        if (context.action.name == controls.Minigame.X.name)
        {
            onButtonX?.Invoke(context.ReadValueAsButton());
        }

        if (context.action.name == controls.Minigame.Y.name)
        {
            onButtonY?.Invoke(context.ReadValueAsButton());
        }

        if (context.action.name == controls.Minigame.A.name)
        {
            onButtonA?.Invoke(context.ReadValueAsButton());
        }

        if (context.action.name == controls.Minigame.B.name)
        {
            onButtonB?.Invoke(context.ReadValueAsButton());
        }

        if (context.action.name == controls.Minigame.LeftTrigger.name)
        {
            onTriggerL?.Invoke(context.ReadValueAsButton());
        }

        if (context.action.name == controls.Minigame.RightTrigger.name)
        {
            onTriggerR?.Invoke(context.ReadValueAsButton());
        }

        if (context.action.name == controls.Minigame.Select.name)
        {
            onSelect?.Invoke(context.ReadValueAsButton());
        }

        if (context.action.name == controls.Minigame.Start.name)
        {
            onStart?.Invoke(context.ReadValueAsButton());
        }

        if (context.action.name == controls.Minigame.Up.name)
        {
            onUp?.Invoke(context.ReadValueAsButton());
        }

        if (context.action.name == controls.Minigame.Down.name)
        {
            onDown?.Invoke(context.ReadValueAsButton());
        }

        if (context.action.name == controls.Minigame.Left.name)
        {
            onLeft?.Invoke(context.ReadValueAsButton());
        }

        if (context.action.name == controls.Minigame.Right.name)
        {
            onRight?.Invoke(context.ReadValueAsButton());
        }

        if (context.action.name == controls.Minigame.Move.name)
        {
            onDPad?.Invoke(context.ReadValue<Vector2>());
        }
    }


}
