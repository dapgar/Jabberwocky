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
        if (instance == null)
        {
            instance = this;
        }

        cameras.Add(GameObject.Find("MainCamera").GetComponent<Camera>());
        cameras.Add(GameObject.Find("PlayerCam1").GetComponent<Camera>());
        cameras.Add(GameObject.Find("PlayerCam2").GetComponent<Camera>());
        cameras.Add(GameObject.Find("PlayerCam3").GetComponent<Camera>());
        cameras.Add(GameObject.Find("PlayerCam4").GetComponent<Camera>());

        players.Add(GameObject.Find("Player (1)").GetComponent<StoneScript>());
        players.Add(GameObject.Find("Player (2)").GetComponent<StoneScript>());
        players.Add(GameObject.Find("Player (3)").GetComponent<StoneScript>());
        players.Add(GameObject.Find("Player (4)").GetComponent<StoneScript>());


        for (int i = 0; i < GameManager.instance.routeData.Length; i++)
        {
            if (GameManager.instance.routeData[i] != 0)
            {
                players[i].routePos = GameManager.instance.routeData[i];
                players[i].transform.position = route.childNodeList[i].position;
            }
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
