using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreboardManager : MonoBehaviour
{
    public static ScoreboardManager instance;

    public bool[] isShowing = new bool[4];

    public GameObject scoreboardObj;
    public Animator animator;

    private void Start()
    {
        if (instance == null) { instance = this; }
    }

    private void Update()
    {
        
    }

    public void ShowScoreboard(bool value)
    {
        if (value)
        {
            scoreboardObj.SetActive(true);
            animator.SetBool("showScoreboard", true);
        }
        else 
        {
            animator.SetBool("showScoreboard", false);
            DisableScoreboard();
        }
    }

    IEnumerator DisableScoreboard()
    {
        yield return new WaitForSeconds(1f);
        scoreboardObj.SetActive(false);
    }
}
