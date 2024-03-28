using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public TextMeshProUGUI hintText;
    private bool hintShown = false;

    private void Start()
    {
        StartCoroutine(WaitToLoad());
    }

    private void Update()
    {
        if (!hintShown)
        {
            StartCoroutine(GameHints());
        }
    }

    IEnumerator WaitToLoad()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(2);
        SceneChanger.Instance.ChangeScene(2);
    }

    IEnumerator GameHints()
    {
        hintShown = true;

        int randomHint = Random.Range(0, 3);

        switch (randomHint)
        {
            case 0:
                hintText.text = "Minigame winners move, losers like you don't. Secure victory to advance!";
                break;

            case 1:
                hintText.text = "Remember, you're playing to win, not to have fun! Wait...";
                break;

            case 2:
                hintText.text = "There are no rules saying you CAN'T steal the winner's controller...";
                break;
        }

        yield return new WaitForSeconds(5f);
        hintShown = false;
    }
}
