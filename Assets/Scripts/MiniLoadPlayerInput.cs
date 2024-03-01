using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniLoadPlayerInput : MonoBehaviour
{
    MiniLoadManager miniLoadManager;
    // Start is called before the first frame update
    void Awake()
    {
        miniLoadManager = GameObject.Find("_MiniLoadManager").GetComponent<MiniLoadManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnButton(bool value)
    {
        // PRESS
        if (value)
        {
            miniLoadManager.TryClickPlay();
        }
    }
}
