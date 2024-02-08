using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActive : MonoBehaviour
{
    [SerializeField]
    private Material readyMaterial;

    public void Enter()
    {
        this.gameObject.GetComponent<MeshRenderer>().material = readyMaterial;
    }
    public void Execute()
    {

    }
    public void Exit()
    {

    }
}
