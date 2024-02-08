using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class StoneScript : MonoBehaviour
{
    // --- VARIABLES ---
    public RouteScript currentRoute;

    public int routePos;
    public bool isMoving;

    public int stoneID;
    public int steps;

    // --- METHODS ---
    public IEnumerator Move(int moveAmount)
    {
        Debug.Log("Player Moving...");
        if (isMoving)
        {
            yield break;
        }

        isMoving = true;
        steps = moveAmount;

        // Target cams
        BoardManager.instance.TurnOffCamerasBut(BoardManager.instance.cameras[stoneID]);

        while (steps > 0) 
        { 
            Vector3 nextPos = currentRoute.childNodeList[routePos + 1].position;

            // Moving to position
            while (MoveToNextNode(nextPos)) { yield return null; }

            yield return new WaitForSeconds(0.2f);
            steps--;
            routePos++;
        }
        isMoving = false;

        BoardManager.instance.TurnOffCamerasBut(BoardManager.instance.cameras[0]);
        transform.LookAt(BoardManager.instance.cameras[0].transform);
        yield return new WaitForSeconds(1f);

        // Return to main cams
    }

    private bool MoveToNextNode(Vector3 target)
    {
        transform.LookAt(target);
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, 2f * Time.deltaTime));
    }
}
