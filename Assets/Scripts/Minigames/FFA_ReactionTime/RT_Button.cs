using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RT_Button : MonoBehaviour
{
    private Renderer btnRenderer;

    [SerializeField]
    private Material inactiveMat;
    [SerializeField]
    private Material readyMat;
    [SerializeField]
    private Material pressedMat;

    void Start()
    {
        btnRenderer = GetComponent<Renderer>();
        SetInactive();
    }

    public void SetInactive()
    {
        btnRenderer.material = inactiveMat;
    }
    public void SetReady()
    {
        btnRenderer.material = readyMat;
    }
    public void SetPressed()
    {
        btnRenderer.material = pressedMat;
    }
}
