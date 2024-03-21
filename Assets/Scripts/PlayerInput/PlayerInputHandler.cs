using System.Collections;
using System.Collections.Generic;
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

    public BoolEvent onButtonA;
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
            return;
        }

        if (context.action.name == controls.Minigame.Button.name)
        {
            onButtonA?.Invoke(context.ReadValueAsButton());
        }

        if (context.action.name == controls.Minigame.Move.name)
        {
            onDPad?.Invoke(context.ReadValue<Vector2>());
        }
    }


}
