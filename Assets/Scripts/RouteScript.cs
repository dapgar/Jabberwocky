using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteScript : MonoBehaviour
{
    // --- VARIABLES ---
    public Transform[] childNodes; // Array of child objects
    public List<Transform> childNodeList = new List<Transform>();

    // --- METHODS ---
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        FillNodes();

        for (int i = 0; i < childNodeList.Count; i++)
        {
            Vector3 currentPos = childNodeList[i].position;
            if (i > 0) 
            {
                Vector3 prevPos = childNodeList[i - 1].position;
                Gizmos.DrawLine(prevPos , currentPos);
            }
        }
    }

    void FillNodes()
    {
        childNodeList.Clear();
        childNodes = GetComponentsInChildren<Transform>();

        foreach (Transform childNode in childNodes) 
        {
            if (childNode != this.transform)
            {
                childNodeList.Add(childNode);
            }
        }
    }
}
