using UnityEngine;

public class RLGL_Character : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    public bool bIsMoving = false;
    public bool bIsAlive = true;
    public bool bIsFinished = false;

    public void SetupCamera(int playerIndex, int totalPlayers) {
        // Only setup for 4 players ATM, need to support 2-4 after MVI
        if (totalPlayers == 4) {
            if (playerIndex == 0) {
                cam.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
            }
            else if (playerIndex == 1) {
                cam.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
            }
            else if (playerIndex == 2) {
                cam.rect = new Rect(0, 0f, 0.5f, 0.5f);
            }
            else if (playerIndex == 3) {
                cam.rect = new Rect(0.5f, 0, 0.5f, 0.5f);
            }
        }
        else if (totalPlayers == 3) {
            if (playerIndex == 0) {
                cam.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
            }
            else if (playerIndex == 1) {
                cam.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
            }
            else {
                cam.rect = new Rect(0, 0f, 1f, 0.5f);
            }
        }
        else {
            if (playerIndex == 0) {
                cam.rect = new Rect(0, 0f, 0.5f, 1f);
            }
            else if (playerIndex == 1) {
                cam.rect = new Rect(0.5f, 0f, 0.5f, 1f);
            }
        }
    }
}
