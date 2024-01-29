using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoneScript : MonoBehaviour
{
    // --- VARIABLES ---
    public RouteScript currentRoute;
    public TextMeshProUGUI diceText;

    private int routePos;
    private bool isMoving;

    public int stoneID;
    public int steps;

    // --- METHODS ---
    private void Update()
    {
        // Rolls Dice
        if (Input.GetKeyDown(KeyCode.Space) && !isMoving) 
        {
            // Dice Logic
            steps = Random.Range(1, 7);
            Debug.Log("Dice Rolled " + steps);

            if (routePos + steps < currentRoute.childNodeList.Count)
            {
                diceText.text = "You Rolled a " + steps + "!";
                StartCoroutine(Move());
            }
            else
            {
                Debug.Log("Rolled Number Is Too High");
            }
        }
    }

    IEnumerator Move()
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

            yield return new WaitForSeconds(0.1f);
            steps--;
            routePos++;
        }

        diceText.text = "'Space' to Roll Dice";
        isMoving = false;
    }

    private bool MoveToNextNode(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, 2f * Time.deltaTime));
    }
}
