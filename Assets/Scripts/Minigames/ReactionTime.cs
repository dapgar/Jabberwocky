using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ReactionTime : MonoBehaviour
{
    float currentTimer;
    float timeGoal;

    [SerializeField]
    private GameObject bigCenterButton;

    [SerializeField]
    private GameObject[] players;

    [SerializeField]
    private TMP_Text timerText;

    [SerializeField]
    private TMP_Text[] scoreTexts;

    [SerializeField] private float minTime;
    [SerializeField] private float maxTime;

    private float[] playerScore;
    private bool[] buttonPressed;
    private int playersLeft;

    public int round;
    private float roundTimer;

    // Start is called before the first frame update
    void Start()
    {
        playerScore = new float[players.Length];
        buttonPressed = new bool[players.Length];
        for (int i = 0; i < buttonPressed.Length; i++)
        {
            buttonPressed[i] = false;
        }

        playersLeft = 0;
        roundTimer = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (roundTimer > 0)
        {
            roundTimer -= Time.deltaTime;
            timerText.text = (Mathf.Ceil(roundTimer)).ToString("F0");
            return;
        }

        if (currentTimer < 0)
        {
            
        }
        else
        {

        }

        PlayerInput(0, Key.Q);
        PlayerInput(1, Key.R);
        PlayerInput(2, Key.U);
        PlayerInput(3, Key.P);

        currentTimer += Time.deltaTime;
        Debug.Log(currentTimer);
        if (currentTimer >= 5)
        {
            playersLeft = 0;
        }

        if (playersLeft <= 0)
        {
            OnRoundEnd();
        }
    }

    private void PlayerInput(int index, Key key)
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return; // no keyboard

        if (keyboard[key].isPressed && !buttonPressed[index])
        {
            playerScore[index] += GetScore();
            scoreTexts[index].text = playerScore[index].ToString("F0");
            playersLeft--;
            buttonPressed[index] = true;
        }
    }

    private int GetScore()
    {
        int score = 0;
        if (currentTimer >= 0)
        {
            score = playersLeft;
        }

        return score;
    }

    private void OnRoundEnd()
    {
        if (round < 3)
        {
            playersLeft = players.Length;
            currentTimer = -Random.Range(minTime, maxTime);
            for (int i = 0; i < buttonPressed.Length; i++)
            {
                buttonPressed[i] = false;
            }
            roundTimer = 3;
            round++;
            Debug.Log("Round " +  round);

            return;
        }

        // End();
    }
}

//StartCoroutine(LightController());


//private void MovePlayer(int index, Key key)
//{
//    Keyboard keyboard = Keyboard.current;
//    if (keyboard == null) return; // no keyboard

//    if (keyboard[key].isPressed)
//    {
//        players[index].transform.Translate(new Vector3(0f, 0f, 1) * speed * Time.deltaTime);
//        movingPlayers[index] = true;
//    }

//    // There is no "key released" so it just checks if they're NOT pressing key but were previously moving, if so sets moving to false
//    if (!keyboard[key].isPressed && movingPlayers[index])
//    {
//        movingPlayers[index] = false;
//    }

//    if (players[index].transform.position.z >= finishLineZ)
//    {
//        // TODO: Actual win & safety for that player, disable movement & face them towards camera?
//        Debug.Log($"Player {index + 1} finished");
//    }
//}

//private IEnumerator LightController()
//{
//    while (runLightCoroutine)
//    {
//        float duration = isLightRed ? Random.Range(minRedLightDuration, maxRedLightDuration) : Random.Range(minGreenLightDuration, maxGreenLightDuration);

//        //Debug.Log($"Light is {(isLightRed ? "Red" : "Green")} for {duration} seconds");

//        StartCoroutine(RotateCoroutine(isLightRed ? 0 : 180));

//        yield return new WaitForSeconds(duration);

//        isLightRed = !isLightRed;
//        lightJustSwitched = true;
//    }
//}

