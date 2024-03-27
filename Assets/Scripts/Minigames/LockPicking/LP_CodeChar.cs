using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LP_CodeChar : MonoBehaviour
{
    [SerializeField]
    GameObject arrow;

    Vector2 dir;
    void Start()
    {
        
    }

    public void Wrong()
    {
        arrow.gameObject.GetComponent<Renderer>().material.color = Color.red;
    }

    public void Unwrong()
    {
        arrow.gameObject.GetComponent<Renderer>().material.color = Color.white;
    }

    public void SetDir(Vector2 direction)
    {
        dir = direction;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
    }

    public bool CheckSolve(Vector2 direction)
    {
        if (dir == direction)
        {
            arrow.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
            return true;
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
