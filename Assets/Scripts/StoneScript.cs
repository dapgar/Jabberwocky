using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class StoneScript : MonoBehaviour
{
    // --- VARIABLES ---
    public RouteScript currentRoute;
    public Vector3 nodeOffset = Vector3.zero;

    public int routePos;
    public bool isMoving;

    public int stoneID;
    public int steps;

    public Animator playerAnim;

    private bool moveFinished = false;
    public bool MoveFinished { get { return moveFinished; }  }

    // --- METHODS ---
    public IEnumerator MovePlayer(int amount, bool skipAnim = false) {
        moveFinished = false;
        isMoving = true;

        // FOR ANIM TESTING
        skipAnim = false;

        if (amount > 0) {
            while (amount > 0) {
                if (!skipAnim) playerAnim.SetTrigger("movePlayer");
                Vector3 nextPos = currentRoute.childNodeList[routePos + 1].position;

                // Moving to position
                while (MoveToNextNode(nextPos, skipAnim)) { yield return null; }

                yield return new WaitForSeconds(skipAnim ? 0.0001f : 0.2f);
                amount--;
                routePos++;
            }

            yield return new WaitForSeconds(skipAnim ? 0.0001f : 1f);
        }
        else if (amount < 0) {
            while (amount < 0) {
                if (routePos == 0) break;
                Vector3 nextPos = currentRoute.childNodeList[routePos - 1].position;

                // Move to position
                while (MoveToNextNode(nextPos, skipAnim)) { yield return null; }

                yield return new WaitForSeconds(skipAnim ? 0.0001f : 0.2f);
                amount++;
                routePos--;
            }

            yield return new WaitForSeconds(skipAnim ? 0.001f : 1f);
        }
        LookAtCamera();
        //if (!skipAnim) playerAnim.SetBool("isMoving", false);

        yield return null;

        // Player's movement is done, tell BoardManager that they're done moving
        moveFinished = true;
        isMoving = false;
    }

    /* Deprecated
    public IEnumerator Move(int moveAmount, bool skipAnim = false)
    {
        if (isMoving)
        {
            yield break;
        }

        isMoving = true;
        if (!skipAnim) playerAnim.SetBool("isMoving", true);
        steps = moveAmount;

        // Target cams
        //BoardManager.instance.TurnOffCamerasBut(BoardManager.instance.cameras[stoneID]);

        while (steps > 0) 
        { 
            Vector3 nextPos = currentRoute.childNodeList[routePos + 1].position;

            // Moving to position
            while (MoveToNextNode(nextPos, skipAnim)) { yield return null; }

            yield return new WaitForSeconds(skipAnim ? 0.0001f : 0.2f);
            steps--;
            routePos++;
        }
        isMoving = false;
        if (!skipAnim) playerAnim.SetBool("isMoving", false);

        // BoardManager.instance.TurnOffCamerasBut(BoardManager.instance.cameras[0]);
        LookAtCamera();
        yield return new WaitForSeconds(skipAnim ? 0.0001f : 1f);

        // Return to main cams
    }
    */

    public void LookAtCamera() {
        transform.LookAt(BoardManager.instance.cam.transform);
    }

    private bool MoveToNextNode(Vector3 target, bool skipAnim)
    {
        transform.LookAt(target);
        float moveSpeed = skipAnim ? 300f : 2f;
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime));
    }
}
