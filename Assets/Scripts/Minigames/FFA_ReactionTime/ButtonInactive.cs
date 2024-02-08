using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInactive : MonoBehaviour, IState
{
    [SerializeField]
    private Material inactiveMaterial;

    public void Enter()
    {
        this.gameObject.GetComponent<MeshRenderer>().material = inactiveMaterial;
    }
    public void Execute()
    {

    }
    public void Exit()
    {

    }
}
