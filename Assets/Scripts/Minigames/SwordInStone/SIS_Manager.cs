using UnityEngine;

public class SIS_Manager : MonoBehaviour {
    [Header("Gameplay Vars")]
    [SerializeField]
    private int maxSwordPulls = 15;
    [SerializeField]
    private int minSwordPulls = 11;
    [SerializeField]
    private SIS_Character[] players;
    [SerializeField]
    private float clickCooldown = 1f;

    [SerializeField]
    private GameObject Sword;

    /* Amount that each click raises the sword */
    private int clickPower = 1;

    private int totalSwordPulls;

    private void Start() {
        totalSwordPulls = Random.Range(minSwordPulls, maxSwordPulls + 1);

        for (int i = 0; i < players.Length; i++) {
            players[i].SetupPlayer(clickCooldown);
        }
    }

    private void Update() {
        for (int i = 0; i < players.Length;i++) {
            players[i].CheckClick();
        }
    }
}
