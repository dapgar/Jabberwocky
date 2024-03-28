using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PauseManager : MonoBehaviour
{
    PostProcessVolume ppVol;
    public bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        ppVol = GameObject.Find("Camera").GetComponent<PostProcessVolume>();
        ppVol.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            isPaused = true;
            ppVol.enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            isPaused = false;
            ppVol.enabled = false;
        }

        if (isPaused)
        {
            // Show pause screen.
        }
    }
}
