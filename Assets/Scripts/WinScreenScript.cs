using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreenScript : MonoBehaviour
{
    public List<Transform> playerPosePos;
    public List<GameObject> players;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance)
        {
            players[GameManager.instance.playerRankings[0].stoneID].transform.position = playerPosePos[0].position;
            players[GameManager.instance.playerRankings[0].stoneID].transform.rotation = playerPosePos[0].rotation;
            players[GameManager.instance.playerRankings[0].stoneID].transform.localScale = playerPosePos[0].localScale;

            players[GameManager.instance.playerRankings[1].stoneID].transform.position = playerPosePos[1].position;
            players[GameManager.instance.playerRankings[1].stoneID].transform.rotation = playerPosePos[1].rotation;
            players[GameManager.instance.playerRankings[1].stoneID].transform.localScale = playerPosePos[1].localScale;

            players[GameManager.instance.playerRankings[2].stoneID].transform.position = playerPosePos[2].position;
            players[GameManager.instance.playerRankings[2].stoneID].transform.rotation = playerPosePos[2].rotation;
            players[GameManager.instance.playerRankings[2].stoneID].transform.localScale = playerPosePos[2].localScale;

            players[GameManager.instance.playerRankings[3].stoneID].transform.position = playerPosePos[3].position;
            players[GameManager.instance.playerRankings[3].stoneID].transform.rotation = playerPosePos[3].rotation;
            players[GameManager.instance.playerRankings[3].stoneID].transform.localScale = playerPosePos[3].localScale;
        }
    }
}
