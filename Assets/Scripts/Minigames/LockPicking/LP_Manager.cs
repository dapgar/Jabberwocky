using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LP_Manager : MonoBehaviour
{
    bool gameOver = false;
    int winner = -1;

    float timer = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (gameOver)
        //{
        //    if (timer <= 0)
        //    {
        //        Win(winner);
        //    }

        //    timer -= Time.deltaTime;
        //}
    }

    public void Winner(int playerIndex)
    {
        if (gameOver)
        {
            return;
        }

        LP_Player[] players = FindObjectsOfType<LP_Player>();

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].playerIndex != playerIndex)
            {
                players[i].Lose();
            }
        }

        gameOver = true;
        winner = playerIndex;

        Win(winner);
    }

    void Win(int playerIndex)
    {
        int[] moveData = new int[GameManager.instance.numPlayers];
        int spacesToMove = GameManager.instance.diceRoll;

        moveData[playerIndex] = spacesToMove;
        GameManager.instance.MoveData(moveData);

        StartCoroutine(ReturnToBoardCoroutine());
    }

    private IEnumerator ReturnToBoardCoroutine()
    {
        yield return new WaitForSeconds(4);

        SceneChanger.Instance.ChangeScene(1);
    }
}
