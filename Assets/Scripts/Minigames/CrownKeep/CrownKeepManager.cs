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

}
