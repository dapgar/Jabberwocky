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
    private float maxPullLength = 0.9f;

    [SerializeField]
    private GameObject Sword;

    /* Amount that each click raises the sword */
    private int totalSwordPulls;
    private float pullAmount;

    private void Start() {
        totalSwordPulls = Random.Range(minSwordPulls, maxSwordPulls + 1);
        pullAmount = maxPullLength / totalSwordPulls;

        for (int i = 0; i < players.Length; i++) {
            players[i].SetupPlayer(clickCooldown, i);
        }

    }

    private void Update() {
        for (int i = 0; i < players.Length;i++) {
            if (players[i].CheckClick()) {
                Vector3 newSwordTransform = Sword.transform.position;
                newSwordTransform.y += pullAmount;
                Sword.transform.position = newSwordTransform;
            }
        }
    }
}
