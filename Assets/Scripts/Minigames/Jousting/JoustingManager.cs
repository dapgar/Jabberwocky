using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoustingManager : MonoBehaviour {
    private List<JoustingCharacter> players;
    private int numPlayers;

    [SerializeField]
    private float speed = 3.0f;

    private void Start() {
        numPlayers = GameManager.instance ? GameManager.instance.numPlayers : 4;
        players = new List<JoustingCharacter>(numPlayers);
    }

    public void SetupPlayer(JoustingCharacter playa) {
        players.Add(playa);
        int playaIndex = players.IndexOf(playa);
        playa.SetupPlayer(playaIndex, speed);
    }

}
