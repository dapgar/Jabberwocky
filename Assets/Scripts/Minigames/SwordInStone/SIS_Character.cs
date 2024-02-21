using UnityEngine;
using UnityEngine.InputSystem;

public class SIS_Character : MonoBehaviour {
    private float clickCooldown;
    private float cooldownTimer = 0f;
    private bool bOnCooldown = false;
    private int playerIndex;
    private Key key;

    public bool OnCooldown { get { return bOnCooldown; } }

    public void SetupPlayer(float clickCooldown, int playerIndex) {
        this.clickCooldown = clickCooldown;
        this.playerIndex = playerIndex;


        SetClickKey();
    }

    private void SetClickKey() {
        if (playerIndex == 0) {
            key = Key.Q;
        }
        else if (playerIndex == 1) {
            key = Key.R;
        }
        else if (playerIndex == 2) {
            key = Key.U;
        }
        else {
            key = Key.P;
        }
    }

    public bool CheckClick() {
        if (bOnCooldown) {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= clickCooldown) {
                cooldownTimer = 0f;
                bOnCooldown = false;
            }
        }
        else {
            Keyboard keyboard = Keyboard.current;
            if (keyboard == null) return false; // if no keyboard

            if (keyboard[key].isPressed) {
                // Click key here
                bOnCooldown = true;
                return true;
            }
        }
        return false;
    }
}
