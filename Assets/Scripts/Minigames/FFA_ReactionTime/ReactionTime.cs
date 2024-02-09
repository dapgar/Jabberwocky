using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ReactionTime : MonoBehaviour
{
    float currentTimer;
    float timeGoal;

    [SerializeField]
    private GameObject bigCenterButton;
    private Button bcbScript;

    [SerializeField]
    private GameObject[] players;

    [SerializeField]
    private GameObject[] playerButtons;
    private Button[] pbScript;

    [SerializeField]
    private TMP_Text timerText;

    [SerializeField]
    private TMP_Text[] scoreTexts;

    [SerializeField] private float minTime;
    [SerializeField] private float maxTime;

    private int[] playerScore;
    private bool[] buttonPressed;
    private int playersLeft;
    private int score;

    public int round;
    private float roundTimer;

    // Start is called before the first frame update
    void Start()
    {
        playerScore = new int[players.Length];
        buttonPressed = new bool[players.Length];
        pbScript = new Button[playerButtons.Length];
        for (int i = 0; i < players.Length; i++)
        {
            buttonPressed[i] = false;
            playerScore[i] = 0;
            players[i].gameObject.transform.LookAt(bigCenterButton.transform);

            pbScript[i] = playerButtons[i].GetComponent<Button>();
        }

        playersLeft = 0;
        score = 0;
        roundTimer = -1;

        bcbScript = bigCenterButton.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playersLeft <= 0)
        {
            OnRoundEnd();
        }

        if (roundTimer > 0)
        {
            roundTimer -= Time.deltaTime;
            timerText.text = (Mathf.Ceil(roundTimer)).ToString("F0");
            return;
        }

        if (currentTimer < 0)
        {
            bcbScript.SetReady();
        }
        else
        {
            bcbScript.SetPressed();
        }

        PlayerInput(0, Key.Q);
        PlayerInput(1, Key.R);
        PlayerInput(2, Key.U);
        PlayerInput(3, Key.P);

        currentTimer += Time.deltaTime;
        if (currentTimer >= 5)
        {
            playersLeft = 0;
            bcbScript.SetInactive();
        }
    }

    private void PlayerInput(int index, Key key)
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return; // no keyboard

        if (keyboard[key].isPressed && !buttonPressed[index])
        {
            playerScore[index] += GetScore();
            if (GetScore() == 0)
            {
                pbScript[index].SetReady();
            }
            else
            {
                pbScript[index].SetPressed();
                score--;
            }
            scoreTexts[index].text = playerScore[index].ToString("F0");
            playersLeft--;
            buttonPressed[index] = true;

            if (playersLeft <= 0)
            {
                currentTimer = 5;
            }
        }
    }

    private int GetScore()
    {
        int scoreToReturn = 0;
        if (currentTimer >= 0)
        {
            scoreToReturn = score;
        }

        return scoreToReturn;
    }

    private void OnRoundEnd()
    {
        if (round < 3)
        {
            playersLeft = players.Length;
            score = playersLeft;
            currentTimer = -Random.Range(minTime, maxTime);
            for (int i = 0; i < buttonPressed.Length; i++)
            {
                buttonPressed[i] = false;
                pbScript[i].SetInactive();
            }
            roundTimer = 3;
            round++;
            Debug.Log("Round " +  round);

            return;
        }

        GameOver();
    }

    private void GameOver()
    {
        int[] moveData = new int[GameManager.instance.numPlayers];
        int spacesToMove = GameManager.instance.diceRoll;

        int highestScore = -1;
        // check to see what the highest score is
        for (int i = 0; i < players.Length; i++)
        {
            if (playerScore[i] > highestScore)
            {
                highestScore = playerScore[i];
            }
        }

        // loop back through to see who moves in case of ties
        for (int i = 0; i < players.Length; i++)
        {
            if (playerScore[i] == highestScore)
            {
                moveData[i] = spacesToMove;
            }
        }

        GameManager.instance.MoveData(moveData);

        // back to board
        StartCoroutine(ReturnToBoardCoroutine());
    }

    private IEnumerator ReturnToBoardCoroutine()
    {
        yield return new WaitForSeconds(4);

        SceneManager.LoadScene(0);
    }
}