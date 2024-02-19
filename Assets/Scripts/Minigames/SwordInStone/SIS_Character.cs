using UnityEngine;

public class SIS_Character : MonoBehaviour {
    private float clickCooldown;
    private float cooldownTimer = 0f;

    public void SetupPlayer(float clickCooldown) {
        this.clickCooldown = clickCooldown;
    }
    
    public void CheckClick() {

    }
}
