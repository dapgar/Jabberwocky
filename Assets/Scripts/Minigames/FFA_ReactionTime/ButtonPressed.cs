using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressed : MonoBehaviour
{
    [SerializeField]
    private Material pressedMaterial;

    public void Enter()
    {
        this.gameObject.GetComponent<MeshRenderer>().material = pressedMaterial;
    }
    public void Execute()
    {

    }
    public void Exit()
    {

    }
}
