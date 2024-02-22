using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public class MiniLoadManager : MonoBehaviour
{
    public int numberOfGames = 2;
    public TextMeshProUGUI hintText;
    public GameObject playButton;

    public TextMeshProUGUI controlText;
    public TextMeshProUGUI titleText;
    public Image minigameImage;

    public GameObject loadIcon;

    private int randomGame;

    private bool hintShown = false;

    private void Start()
    {
        playButton.SetActive(false);
        loadIcon.SetActive(true);

        randomGame = Random.Range(0, numberOfGames);
        StartCoroutine(WaitToLoad());

        // Image/ Text Updating
        UpdateScreenContent();
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

        // Loads minigame scene accounting for menus.
        SceneChanger.Instance.ChangeScene(randomGame + 4);
    }

    IEnumerator WaitToLoad()
    {
        yield return new WaitForSeconds(3f);
        playButton.SetActive(true);
        loadIcon.SetActive(false);
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

        yield return new WaitForSeconds(4f);
        hintShown = false;
    }

    private void UpdateScreenContent()
    {
        switch (randomGame)
        {
            case 0:
                break;

            case 1:
                break;

            case 2:
                break;
        }
    }
}
