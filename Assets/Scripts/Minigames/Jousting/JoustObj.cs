using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoustObj : MonoBehaviour
{
    [SerializeField]
    JoustingCharacter jouster;

    private void OnTriggerEnter(Collider other)
    {
        JoustingCharacter otherJouster = other.gameObject.gameObject.GetComponent<JoustingCharacter>();
        if (otherJouster != null)
        {
            otherJouster.GetHit();
            //Vector3 direction = other.transform.position - transform.position;
            //float dotProduct = Vector3.Dot(transform.forward, direction.normalized);
            //if (dotProduct > 0)
            //{
            //    //Debug.Log("Trigger Hit from the front");
            //    //jouster.ShieldHit();
            //}
            //else
            //{
            //    Debug.Log("Trigger Hit from the back");
            //    otherJouster.GetHit();
            //}
        }
        else
        {
            Vector3 direction = other.transform.position - transform.position;
            float dotProduct = Vector3.Dot(transform.forward, direction.normalized);
            if (dotProduct > 0)
            {
                //Debug.Log("Trigger Hit from the front");
                jouster.ShieldHit();
            }
            //jouster.ShieldHit();
        }
    }
}
