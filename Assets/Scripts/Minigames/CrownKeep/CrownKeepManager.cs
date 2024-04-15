using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrownKeepManager : MonoBehaviour {
    private List<CrownKeepCharacter> players;

    private void Start() {
        players = new List<CrownKeepCharacter>();
    }
    public void GetPlayers(CrownKeepCharacter player) {
        players.Add(player);
    }

}
