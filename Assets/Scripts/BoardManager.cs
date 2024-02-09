using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;

    public List<StoneScript> players;
    public List<Camera> cameras;

    public RouteScript route;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) instance = this;

        cameras.Add(GameObject.Find("MainCamera").GetComponent<Camera>());
        cameras.Add(GameObject.Find("PlayerCam1").GetComponent<Camera>());
        cameras.Add(GameObject.Find("PlayerCam2").GetComponent<Camera>());
        cameras.Add(GameObject.Find("PlayerCam3").GetComponent<Camera>());
        cameras.Add(GameObject.Find("PlayerCam4").GetComponent<Camera>());

        players.Add(GameObject.Find("Player (1)").GetComponent<StoneScript>());
        players.Add(GameObject.Find("Player (2)").GetComponent<StoneScript>());
        players.Add(GameObject.Find("Player (3)").GetComponent<StoneScript>());
        players.Add(GameObject.Find("Player (4)").GetComponent<StoneScript>());

        for (int i = 0; i < GameManager.instance.playersPos.Length; i++) {
            players[i].routePos = GameManager.instance.routeData[i];
            if (GameManager.instance.playersPos[i] != Vector3.zero) players[i].transform.position = GameManager.instance.playersPos[i];
            if (GameManager.instance.playerRots[i] != Quaternion.identity)  players[i].transform.rotation = GameManager.instance.playerRots[i];
        }

        // Old using routes instead of Vector & Quaternion
        /*for (int i = 0; i < GameManager.instance.routeData.Length; i++)
        {
            if (GameManager.instance.routeData[i] != 0)
            {
                players[i].routePos = GameManager.instance.routeData[i];
                players[i].transform.position = route.childNodeList[i].position;
            }
        }*/

        foreach (StoneScript p in players) {
            p.LookAtCamera();
        }

        StartCoroutine(UpdateBoard());
    }

    IEnumerator UpdateBoard()
    {
        if (GameManager.instance)
        {
            Debug.Log("GameManager Exists");
            TurnOffCamerasBut(cameras[0]);
        }

        yield return new WaitForSeconds(3f);
        Debug.Log("Updating Player Positions...");

        foreach(StoneScript player in players)
        {
            Debug.Log("Moving Player " +  player.stoneID);
            if ((GameManager.instance.moveData[player.stoneID - 1] > 0))
            {
                StartCoroutine(player.Move(GameManager.instance.moveData[player.stoneID - 1]));
                while (player.isMoving) { yield return null; }

                yield return new WaitForSeconds(1f);
            }
            GameManager.instance.routeData[player.stoneID - 1] = player.routePos;
            //GameManager.instance.playersPos[player.stoneID - 1] = player.transform.position;
        }

        // Used for storing prev board pos / rot
        // Can clean this up AFTER MVI - don't wanna mess smth up rn lol
        for (int i = 0; i < players.Count; i++) {
            GameManager.instance.playersPos[i] = players[i].transform.position;
            GameManager.instance.playerRots[i] = players[i].transform.rotation;
        }
    }

    public void TurnOffCamerasBut(Camera camToKeep)
    {
        foreach (Camera c in cameras)
        {
            c.enabled = false;
        }

        camToKeep.enabled = true;

    }
}
