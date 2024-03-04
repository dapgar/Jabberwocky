using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPlayerInput : MonoBehaviour
{
    SceneChanger sceneChanger;
    // Start is called before the first frame update
    void Awake()
    {
        sceneChanger = GameObject.Find("_SceneChanger").GetComponent<SceneChanger>();
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
            sceneChanger.ChangeScene(1);
        }
    }
}
