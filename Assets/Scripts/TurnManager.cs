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

    public TextMeshProUGUI diceTutText;
    public TextMeshProUGUI rollText;

    public int result;

    private bool canStartNextTurn = true;

    // Start is called before the first frame update
    void Start()
    {
        diceAnimObj.SetActive(false);
        dice.SetActive(false);
        rollText.enabled = false;
        diceTutText.enabled = true;
    }

    private void Update()
    {
        // Rolls Dice
        if (Input.GetKeyDown(KeyCode.Space) && !isRolling && canStartNextTurn)
        {
            StartCoroutine(StartTurn());
        }
        // TEMP QUIT GAME
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }

    IEnumerator StartTurn()
    {
        canStartNextTurn = false;

        // UI
        diceTutText.enabled = false;

        // Dice Logic
        diceAnimObj.SetActive(true);

        result = RollDice();
        yield return new WaitForSeconds(3f);
        StartCoroutine(MenuTransition(result));
    }

    private int RollDice()
    {
        isRolling = true;
        diceAnim.SetBool("isRolling", true);

        int diceRoll;
        diceRoll = Random.Range(1, 7);

        return diceRoll;
    }

    IEnumerator MenuTransition(int faceNumber)
    {
        diceAnimObj.SetActive(false);
        diceAnim.SetBool("isRolling", false);

        dice.SetActive(true);

        Debug.Log("Rolled a " + faceNumber);
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
        SceneManager.LoadScene(1); // Hard coded for now
    }


}
