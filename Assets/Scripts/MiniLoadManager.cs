using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public class MiniLoadManager : MonoBehaviour
{
    public TextMeshProUGUI hintText;
    public GameObject playButton;

    public TextMeshProUGUI controlText;
    public TextMeshProUGUI titleText;
    public Image minigameImage;
    public Texture2D[] images;

    public GameObject loadIcon;

    private int randomGame;

    private bool hintShown = false;

    private void Start() {
        playButton.SetActive(false);
        loadIcon.SetActive(true);

        if (GameManager.instance && GameManager.instance.devMinigameNumber != -1) {
            // DEV TOOLS: Pick seleted minigame
            randomGame = GameManager.instance.devMinigameNumber;
            GameManager.instance.devMinigameNumber = -1;
        }
        else {
            //randomGame = Random.Range(0, numberOfGames);

            randomGame = GameManager.instance.RandomGame();
        }
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

        if (Input.GetKeyDown(KeyCode.Space) && playButton.activeSelf) {
            OnPlayClicked();
        }

    }

    // called by player input scripts
    public void TryClickPlay()
    {
        if (playButton.activeSelf)
        {
            OnPlayClicked();
        }
    }

    public void OnPlayClicked()
    {

        // Loads minigame scene accounting for menus.
        SceneChanger.Instance.ChangeScene(randomGame + 5);
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
                hintText.text = "Minigame winners move, losers like you don't. Win to advance!";
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
                titleText.text = "Red Light, Green Light";
                controlText.text = "Hold A to move to the finish line before your opponents";
                minigameImage.sprite = Sprite.Create(images[0], new Rect(0, 0, images[0].width, images[0].height), Vector2.zero);
                break;

            case 1:
                titleText.text = "Quick Reaction";
                controlText.text = "Press A when the center flashes green before your opponents";
                minigameImage.sprite = Sprite.Create(images[1], new Rect(0, 0, images[1].width, images[1].height), Vector2.zero);
                break;

            case 2:
                titleText.text = "Swords in the Stone";
                controlText.text = "Mash A to wiggle the sword out of the stone";
                minigameImage.sprite = Sprite.Create(images[2], new Rect(0, 0, images[2].width, images[2].height), Vector2.zero);
                break;

            case 3:
                titleText.text = "Lockpicking";
                controlText.text = "Press the arrow keys to pick the locks";
                minigameImage.sprite = Sprite.Create(images[3], new Rect(0, 0, images[3].width, images[3].height), Vector2.zero);
                break;
            case 4:
                titleText.text = "Crown Keep";
                controlText.text = "Use the d-pad to move around and keep the crown for yourself for as long as possible";
                minigameImage.sprite = Sprite.Create(images[4], new Rect(0, 0, images[4].width, images[4].height), Vector2.zero);
                break;
            case 5:
                titleText.text = "Jousting";
                controlText.text = "Use the up/down arrows to accelerate/decelerate and the left/right arrows to turn. Try to hit your opponents, while avoiding their shields.";
                minigameImage.sprite = Sprite.Create(images[5], new Rect(0, 0, images[5].width, images[5].height), Vector2.zero);
                break;
        }
    }
}
