using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CrownKeepManager : MonoBehaviour {
    private List<CrownKeepCharacter> players;

    [SerializeField]
    private float gameTimer = 45f;
    [SerializeField]
    private TMP_Text timerText;

    private float preGameTimer = 2f;
    bool bInGame = false;

    private int currentCrownHolder = -1;

    [SerializeField]
    private GameObject crown;

    private bool bPlayersCanMove = false;
    public bool PlayersCanMove { get { return bPlayersCanMove; } }

    private void Start() {
        players = new List<CrownKeepCharacter>();

    }
    public void GetPlayers(CrownKeepCharacter player) {
        players.Add(player);
    }

    private void Update() {
        if (bInGame) {
            gameTimer -= Time.deltaTime;
            timerText.text = $"Time Remaining: {gameTimer.ToString("F1")}";
        }
        else {
            // In pregame
            preGameTimer -= Time.deltaTime;
            if (preGameTimer <= 0f) {
                bInGame = true;
                preGameTimer = 0f;
                bPlayersCanMove = true;
            }
            timerText.text = $"Game Starts In: {preGameTimer.ToString("F1")}";

        }
    }

    public void OnCrownTouched(CrownKeepCharacter player) {
        Debug.Log("Crown touched");
        int index = players.IndexOf(player);

        // Initial Crown Pickup currentCrownHolder == -1, so guy who picks it up skips other checks
        if (currentCrownHolder == -1) {
            currentCrownHolder = index;
            player.GotCrown();
            AttachCrownToPlayer(player);
            crown.GetComponent<Animator>().enabled = false;
            return;
        }

        if (index == -1 || index == currentCrownHolder || !players[currentCrownHolder].CanGetStolen()) {
            return;
        }

        players[currentCrownHolder].CrownStolen();
        currentCrownHolder = index;
        player.GotCrown();
        AttachCrownToPlayer(player);
    }

    private void AttachCrownToPlayer(CrownKeepCharacter player) {
        crown.transform.parent = player.transform;
        crown.transform.localPosition = new Vector3(0f, 0.6f, 0f);
        crown.transform.localRotation = Quaternion.identity;
    }

}
