using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;
    PostProcessVolume ppVol;

    public Canvas pauseCanvas;
    public Canvas normalCanvas;
    public bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        ppVol = GameObject.Find("Camera").GetComponent<PostProcessVolume>();
        ppVol.enabled = false;

        pauseCanvas.gameObject.SetActive(false);
        normalCanvas.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            Pause();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            Unpause();
        }
    }

    public void Pause()
    {
        isPaused = true;
        ppVol.enabled = true;
        normalCanvas.gameObject.SetActive(false);
        pauseCanvas.gameObject.SetActive(true);
    }

    public void Unpause()
    {
        isPaused = false;
        ppVol.enabled = false;
        normalCanvas.gameObject.SetActive(true);
        pauseCanvas.gameObject.SetActive(false);
    }
}
