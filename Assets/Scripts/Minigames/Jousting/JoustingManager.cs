using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoustingManager : MonoBehaviour {
    private List<JoustingCharacter> players;
    private int numPlayers;



    [SerializeField]
    private GameEvent onWin;

    [SerializeField]
    private float speed = 3.0f;

    [SerializeField]
    private float turnSpeed = 2.0f;

    private void Start() {
        numPlayers = GameManager.instance ? GameManager.instance.numPlayers : 2;
        players = new List<JoustingCharacter>(numPlayers);
    }

    public void SetupPlayer(JoustingCharacter playa) {
        players.Add(playa);
        int playaIndex = players.IndexOf(playa);
        playa.SetupPlayer(playaIndex, speed, turnSpeed);
    }

    public void OnPlayerDie()
    {
        Debug.Log("A player has died!");
        numPlayers--;
        if (numPlayers <= 1)
        {
            Debug.Log("One Left Standing");
            onWin.Invoke();
        }
    }

    IEnumerator WinPlayer()
    {
        yield return new WaitForSeconds(4);
        players.RemoveAll(players => players == null);
        int[] moveData = new int[GameManager.instance.numPlayers];
        int spacesToMove = GameManager.instance.diceRoll;
        moveData[players[0].playerIndex] = spacesToMove;

        SceneChanger.Instance.ChangeScene(1);
    }
}
