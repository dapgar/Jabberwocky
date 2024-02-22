using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;

    public List<StoneScript> players;
    public GameObject[] crowns;

    public CinemachineVirtualCamera cam;
    private Vector3 camDefaultPos;
    private Quaternion camDefaultRot;

    public RouteScript route;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) instance = this;

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

        camDefaultPos = new Vector3(0.7f, 4.5f, 4.5f);
        camDefaultRot = Quaternion.Euler(50f, 244, 0);

        StartCoroutine(UpdateBoard());
    }

    IEnumerator UpdateBoard()
    {
        /* Used to disable dice roll during player moves */
        GameManager.instance.playersMoving = true;

        yield return new WaitForSeconds(3f);
       
        // Moves players after each game.
        foreach(StoneScript player in players)
        {
            if ((GameManager.instance.moveData[player.stoneID - 1] > 0))
            {
                StartCoroutine(player.Move(GameManager.instance.moveData[player.stoneID - 1]));
                PointCam(player.transform);
                while (player.isMoving) { yield return null; }
                ResetCam();
                yield return new WaitForSeconds(1f);
            }
            GameManager.instance.routeData[player.stoneID - 1] = player.routePos;
            //GameManager.instance.playersPos[player.stoneID - 1] = player.transform.position;
        }
        GameManager.instance.playersMoving = false;

        /*// Handles crown logic.
        for (int i = 0; i < crowns.Length; i++)
        {
            if (players[i].routePos > furthestPlayer)
            {
                furthestPlayer = i;
                crowns[furthestPlayer].SetActive(true);
            }
            else
            {
                crowns[i].SetActive(false);
            }
        }*/

        // Used for storing prev board pos / rot
        // Can clean this up AFTER MVI - don't wanna mess smth up rn lol
        for (int i = 0; i < players.Count; i++) {
            GameManager.instance.playersPos[i] = players[i].transform.position;
            GameManager.instance.playerRots[i] = players[i].transform.rotation;
        }
    }

    private void PointCam(Transform target)
    {
        cam.LookAt = target;
        cam.m_Lens.FieldOfView = 30;
    }

    private void ResetCam()
    {
        cam.LookAt = null;
        cam.transform.SetPositionAndRotation(camDefaultPos, camDefaultRot);
        cam.m_Lens.FieldOfView = 70;
    }
}


