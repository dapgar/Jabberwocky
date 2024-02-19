using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MiniLoadManager : MonoBehaviour
{
    public int numberOfGames = 2;
    public TextMeshProUGUI hintText;
    public GameObject playButton;

    private bool hintShown = false;

    private void Start()
    {
        playButton.SetActive(false);
        StartCoroutine(WaitToLoad());
    }

    private void Update()
    {
        if (!hintShown)
        {
            StartCoroutine(GameHints());
            hintShown = true;
        }
    }

    public void OnPlayClicked()
    {
        int randomGame = Random.Range(0, numberOfGames);

        // Loads minigame scene accounting for menus.
        SceneManager.LoadScene(randomGame + 4);
    }

    IEnumerator WaitToLoad()
    {
        yield return new WaitForSeconds(3f);
        playButton.SetActive(true);
    }

    IEnumerator GameHints()
    {
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
