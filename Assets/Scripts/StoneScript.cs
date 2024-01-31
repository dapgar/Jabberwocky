using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoneScript : MonoBehaviour
{
    // --- VARIABLES ---
    public RouteScript currentRoute;

    public int routePos;
    public bool isMoving;

    public int stoneID;
    public int steps;

    // --- METHODS ---
    public IEnumerator Move()
    {
        if (isMoving)
        {
            yield break;
        }
        isMoving = true;

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
    }

    private bool MoveToNextNode(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, 2f * Time.deltaTime));
    }
}
