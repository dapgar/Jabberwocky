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
        //numPlayers = GameManager.instance ? GameManager.instance.numPlayers : 4;
        //Debug.Log(numPlayers);
        players = new List<JoustingCharacter>();
    }

    public void SetupPlayer(JoustingCharacter playa) {
        numPlayers++;
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
            StartCoroutine(WinPlayer());
        }
    }

    IEnumerator WinPlayer()
    {
        yield return new WaitForSeconds(1.5f);
        players.RemoveAll(players => players == null);
        int[] moveData = new int[GameManager.instance.numPlayers];
        int spacesToMove = GameManager.instance.diceRoll;
        moveData[players[0].playerIndex] = spacesToMove;
        GameManager.instance.MoveData(moveData);

        SceneChanger.Instance.ChangeScene(1);
    }
}
