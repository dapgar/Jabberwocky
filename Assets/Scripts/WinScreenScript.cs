using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreenScript : MonoBehaviour
{
    public List<Transform> playerPosePos;
    public List<GameObject> players;

    private bool playersPlaced;

    // Start is called before the first frame update
    private void Update()
    {
        if (GameManager.instance && !playersPlaced)
        {
            players[GameManager.instance.playerRankings[0]].transform.SetPositionAndRotation(playerPosePos[0].position, playerPosePos[0].rotation);
            players[GameManager.instance.playerRankings[0]].transform.localScale = playerPosePos[0].localScale;

            players[GameManager.instance.playerRankings[1]].transform.SetPositionAndRotation(playerPosePos[1].position, playerPosePos[1].rotation);
            players[GameManager.instance.playerRankings[1]].transform.localScale = playerPosePos[1].localScale;

            players[GameManager.instance.playerRankings[2]].transform.SetPositionAndRotation(playerPosePos[2].position, playerPosePos[2].rotation);
            players[GameManager.instance.playerRankings[2]].transform.localScale = playerPosePos[2].localScale;

            players[GameManager.instance.playerRankings[3]].transform.SetPositionAndRotation(playerPosePos[3].position, playerPosePos[3].rotation);
            players[GameManager.instance.playerRankings[3]].transform.localScale = playerPosePos[3].localScale;
        }
    }
}
