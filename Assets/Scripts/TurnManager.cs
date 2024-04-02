using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TurnManager : MonoBehaviour
{
    public GameObject dice;
    public Transform dicePivot;
    public GameObject diceAnimObj;
    public Animator diceAnim;

    public bool isRolling;

    public GameObject diceTutText;
    public TextMeshProUGUI rollText;

    public int result;

    private bool canStartNextTurn = true;

    // Start is called before the first frame update
    void Start()
    {
        diceAnimObj.SetActive(false);
        dice.SetActive(false);
        rollText.enabled = false;
        diceTutText.SetActive(true);
    }

    private void Update()
    {
        if (GameManager.instance.playersMoving || isRolling) diceTutText.SetActive(false);
        else diceTutText.SetActive(true);

        // Rolls Dice
        if (Input.GetKeyDown(KeyCode.Space) && !isRolling && canStartNextTurn && !GameManager.instance.playersMoving && !PauseManager.instance.isPaused)
        {
            StartCoroutine(StartTurn());
        }
    }

    // called by player input scripts
    public void TryStartTurn()
    {
        if (!isRolling && canStartNextTurn && !GameManager.instance.playersMoving)
        {
            StartCoroutine (StartTurn());
        }
    }

    IEnumerator StartTurn()
    {
        canStartNextTurn = false;

        // UI
        diceTutText.SetActive(false);

        // Dice Logic
        diceAnimObj.SetActive(true);
        diceAnim.SetBool("isRolling", true);

        result = RollDice();
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(MenuTransition(result));
    }

    private int RollDice()
    {
        isRolling = true;

        int diceRoll;
        diceRoll = Random.Range(1, 7);

        return diceRoll;
    }

    IEnumerator MenuTransition(int faceNumber)
    {
        diceAnimObj.SetActive(false);
        diceAnim.SetBool("isRolling", false);

        dice.SetActive(true);

        switch (faceNumber)
        {
            case 1:
                dicePivot.Rotate(0, 90, 0);
                break;

            case 2:
                dicePivot.Rotate(0, 90, -90);
                break;

            case 3:
                dicePivot.Rotate(0, 180, -90);
                break;

            case 4:
                dicePivot.Rotate(0, 360, -90);
                break;

            case 5:
                dicePivot.Rotate(0, 270, -90);
                break;

            case 6:
                dicePivot.Rotate(0, 270, 0);
                break;

            default:
                break;
        }

        yield return new WaitForSeconds(3f);

        // Stores Dice Roll
        GameManager.instance.diceRoll = result;

        // Loads Load Scene
        SceneChanger.Instance.ChangeScene(4);
    }


}
