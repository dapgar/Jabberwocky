using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List <Camera> cameras;
    public List<StoneScript> players;
    public int diceRoll;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!instance)
        {
            instance = this;
        }
    }



    public void TurnOffCamerasBut(Camera camToKeep)
    {
        foreach (Camera c in cameras) 
        {
            c.enabled = false;
        }

        camToKeep.enabled = true;
    }
}
