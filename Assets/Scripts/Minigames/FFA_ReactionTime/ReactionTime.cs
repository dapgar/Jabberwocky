using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ReactionTime : MonoBehaviour
{
    enum GameState {
        roundStart,
        red,
        green,
        roundEnd
    }
    GameState gameState;

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

    GameObject playerScored;

    public CountdownTimer roundStartTimer;
    public RandomReactionTimer ongoingTimer;
    public CountdownTimer roundEndTimer;

    public int round;

    IState currentState;

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.roundStart;

        roundStartTimer.onTimerEnd += () => OnRoundStart();
        ongoingTimer.onTimerEnd += () => OnLightGreen();
        roundEndTimer.onTimerEnd += () => OnRoundEnd();

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

        bcbScript = bigCenterButton.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.roundStart:
                roundStartTimer.Countdown();
                timerText.text = (Mathf.Ceil(roundStartTimer.time)).ToString("F0");
                break;
            case GameState.red:
                if (playersLeft <= 0)
                {
                    OnRoundScored();
                }

                PlayerInput(0, Key.Q);
                PlayerInput(1, Key.R);
                PlayerInput(2, Key.U);
                PlayerInput(3, Key.P);

                ongoingTimer.Countdown();
                break;
            case GameState.green:
                if (playersLeft <= 0 || playerScored != null)
                {
                    OnRoundScored();
                }

                PlayerInput(0, Key.Q);
                PlayerInput(1, Key.R);
                PlayerInput(2, Key.U);
                PlayerInput(3, Key.P);
                break;
            case GameState.roundEnd:
                if (playerScored != null)
                {
                    playerScored.transform.Rotate(new Vector3(Time.deltaTime * 240, 0, 0), Space.Self);
                    //playerScored.transform.position += new Vector3(0, Time.deltaTime, 0);
                }
                roundEndTimer.Countdown();
                break;
        }


        //if (roundTimer > 0)
        //{
        //    roundTimer -= Time.deltaTime;
        //    timerText.text = (Mathf.Ceil(roundTimer)).ToString("F0");
        //    return;
        //}

        
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
                playerScored = players[index];
                pbScript[index].SetPressed();
                score--;
            }
            scoreTexts[index].text = playerScore[index].ToString("F0");
            playersLeft--;
            buttonPressed[index] = true;
        }
    }

    private int GetScore()
    {
        int scoreToReturn = 0;
        if (gameState == GameState.green && playerScored == null)
        {
            scoreToReturn = 1;
        }

        return scoreToReturn;
    }

    public void OnRoundStart()
    {
        playersLeft = players.Length;

        gameState = GameState.red;
        bcbScript.SetReady();
        ongoingTimer.Reset();

        timerText.gameObject.SetActive(false);

    }

    public void OnLightGreen()
    {
        bcbScript.SetPressed();

        gameState = GameState.green;

    }

    private void OnRoundScored()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (!buttonPressed[i])
            {
                buttonPressed[i] = true;
                pbScript[i].SetReady();
            }
        }

        roundEndTimer.Reset();

        gameState = GameState.roundEnd;
    }

    public void OnRoundEnd()
    {
        int highestScore = -1;
        for (int i = 0; i < players.Length; i++)
        {
            if (playerScore[i] > highestScore)
            {
                highestScore = playerScore[i];
            }
        }

        if (highestScore < 3)
        {
            bcbScript.SetInactive();
            playersLeft = players.Length;

            gameState = GameState.roundStart;
            roundStartTimer.Reset();
            round++;

            playerScored = null;
            for (int i = 0; i < buttonPressed.Length; i++)
            {
                buttonPressed[i] = false;
                pbScript[i].SetInactive();
            }

            timerText.gameObject.SetActive(false);

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

        SceneChanger.Instance.ChangeScene(1);
    }

    public void ChangeState(IState newState)
    {
        currentState?.OnExit();
        currentState = newState;
        currentState.OnEnter();
    }

    public void HandlePlayerInput(ReactionTimePlayer player)
    {
        int index = player.playerIndex;
        if ((gameState == GameState.red || gameState == GameState.green) && !buttonPressed[index])
        {
            playerScore[index] += GetScore();
            if (GetScore() == 0)
            {
                player.Die();
                pbScript[index].SetReady();
            }
            else
            {
                player.Backflip();
                playerScored = players[index];
                pbScript[index].SetPressed();
                score--;
            }
            scoreTexts[index].text = playerScore[index].ToString("F0");
            playersLeft--;
            buttonPressed[index] = true;
        }
    }

    public void SetupPlayer(ReactionTimePlayer player)
    {
        player.Setup();
    }
}