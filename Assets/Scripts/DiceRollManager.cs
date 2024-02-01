using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiceRollManager : MonoBehaviour
{
    public GameObject dice;
    public bool isRolling;

    public TextMeshProUGUI diceTutText;
    public TextMeshProUGUI rollText;
    public Image diceFace;
    public List<Sprite> diceFaceImages;

    public Image smallDiceFace;

    public int result;

    private bool canStartNextTurn = true;

    // Start is called before the first frame update
    void Start()
    {
        dice.SetActive(false);
        diceFace.enabled = false;
        rollText.enabled = false;
        diceTutText.enabled = true;
        smallDiceFace.enabled = false;
    }

    private void Update()
    {
        // Rolls Dice
        if (Input.GetKeyDown(KeyCode.Space) && !isRolling && canStartNextTurn)
        {
            StartCoroutine(StartTurn());
        }
    }

    IEnumerator StartTurn()
    {
        canStartNextTurn = false;

        // UI
        diceTutText.enabled = false;

        // Dice Logic
        dice.SetActive(true);

        yield return new WaitForSeconds(3f);
        result = RollDice();
        ChangeDiceFace(result);
    }

    private int RollDice()
    {
        isRolling = true;
        int diceRoll;
        diceRoll = Random.Range(0, 7);

        return diceRoll;
    }

    private void ChangeDiceFace(int faceNumber)
    {
        dice.SetActive(false);
        diceFace.enabled = true;

        diceFace.sprite = diceFaceImages[faceNumber - 1];
        smallDiceFace.sprite = diceFaceImages[faceNumber - 1];

        rollText.enabled = true;
        smallDiceFace.enabled = true;
    }


}
