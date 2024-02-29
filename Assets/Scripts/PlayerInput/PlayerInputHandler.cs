using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerConfiguration playerConfig;

    private GameObject playerChar;

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
        playerConfig.Input.onActionTriggered += Input_onActionTriggered;
    }

    private void Input_onActionTriggered(CallbackContext context)
    {
        if (context.action.name == controls.Minigame.Button.name)
        {
            onButtonA?.Invoke(context.ReadValueAsButton());
        }
    }


}
