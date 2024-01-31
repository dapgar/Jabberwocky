using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoneScript : MonoBehaviour
{
    // --- VARIABLES ---
    public RouteScript currentRoute;
    public TextMeshProUGUI diceText;
    public DiceRollManager drm;

    private int routePos;
    private bool isMoving;

    public int stoneID;
    public int steps;

    // --- METHODS ---
    private void Start()
    {
        drm = GameObject.Find("_DiceRollManager").GetComponent<DiceRollManager>();
    }

    private void Update()
    {
        // Rolls Dice
        if (Input.GetKeyDown(KeyCode.Space) && !isMoving && !drm.isRolling) 
        {
            StartCoroutine(RollAndMove());
        }
    }

    IEnumerator RollAndMove()
    {
        // Dice Logic
        drm.RollDice();
        yield return new WaitForSeconds(4f);
        steps = drm.DetectResult();

        // Movement Logic
        if (routePos + steps < currentRoute.childNodeList.Count)
        {
            StartCoroutine(Move());
        }
        else
        {
            Debug.Log("Rolled Number Is Too High");
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

            yield return new WaitForSeconds(0.2f);
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
