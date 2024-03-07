using UnityEngine;

public class SIS_Character : MonoBehaviour {
    private Animator animator;

    private float maxStamina;
    private float stamina;
    private float staminaRegenRate;
    private float staminaClickDrain;
    private bool staminaDepleted = false;
    private bool keyDownPrevFrame = false;

    private bool bButtonPressed = false;
    public float Stamina { get { return stamina; } }
    public bool IsButtonPressed {  get { return bButtonPressed; } }

    private SIS_Manager manager;
    private void Start() {
        manager = GameObject.Find("SIS_Manager").GetComponent<SIS_Manager>();
        manager.SetupPlayer(this);
    }

    public void SetupPlayer(int playerIndex, float maxStamina, float staminaRegenRate, float staminaClickDrain) {
        this.maxStamina = maxStamina;
        this.staminaRegenRate = staminaRegenRate;
        this.staminaClickDrain = staminaClickDrain;

        stamina = maxStamina;
        keyDownPrevFrame = false;

        Transform playerTransform = transform.Find($"Player{playerIndex + 1}(Clone)");
        animator = playerTransform.GetComponent<Animator>();
    }

    public void OnButtonA(bool value) {
        bButtonPressed = value;
    }

    public bool CheckClick() {
        stamina += staminaRegenRate * Time.deltaTime;
        if (stamina >= maxStamina) {
            staminaDepleted = false;
            stamina = maxStamina;
        }
        
        if (staminaDepleted) {
            return false;
        }

        /*Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return false; // if no keyboard*/

        if (bButtonPressed) {
            if (keyDownPrevFrame) return false;

            stamina -= staminaClickDrain;
            if (stamina <= 0f) {
                staminaDepleted = true;
                stamina = 0f;
            }

            keyDownPrevFrame = true;

            if (animator) animator.SetTrigger("SwordPull");

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
