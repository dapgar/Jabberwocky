using UnityEngine;
using UnityEngine.InputSystem;

public class SIS_Character : MonoBehaviour {
    [SerializeField]
    private Animator animator;
    
    private int playerIndex;
    private Key key;

    private float maxStamina;
    private float stamina;
    private float staminaRegenRate;
    private float staminaClickDrain;
    private bool staminaDepleted = false;
    private bool keyDownPrevFrame = false;

    public float Stamina { get { return stamina; } }

    public void SetupPlayer(int playerIndex, float maxStamina, float staminaRegenRate, float staminaClickDrain) {
        this.playerIndex = playerIndex;
        this.maxStamina = maxStamina;
        this.staminaRegenRate = staminaRegenRate;
        this.staminaClickDrain = staminaClickDrain;

        stamina = maxStamina;
        keyDownPrevFrame = false;

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
        // If stamina is fully depleted, regen at 2x the rate
        stamina += staminaDepleted ? staminaRegenRate * 2 * Time.deltaTime : staminaRegenRate * Time.deltaTime;
        if (stamina >= maxStamina) {
            staminaDepleted = false;
            stamina = maxStamina;
        }
        if (playerIndex == 0) Debug.Log("STAMINA: " + stamina);
        if (staminaDepleted) {
            return false;
        }

        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return false; // if no keyboard

        if (keyboard[key].isPressed) {
            if (keyDownPrevFrame) return false;

            stamina -= staminaClickDrain;
            if (stamina <= 0f) {
                staminaDepleted = true;
                stamina = 0f;
            }

            keyDownPrevFrame = true;

            if (animator) animator.SetTrigger("PullSword");

            // SIS_Manager handles actual pulling of the sword
            // Just return true here to let SIS_Manager know that this click pulled the sword
            return true;
        }
        else {
            keyDownPrevFrame = false;
        }

        return false;
    }
}
