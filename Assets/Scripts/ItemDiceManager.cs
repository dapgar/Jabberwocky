using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemDiceManager : MonoBehaviour
{
    public static ItemDiceManager Instance;

    public GameObject itemDice;
    public Transform itemDicePivot;
    public GameObject itemDiceAnimObj;
    public Animator itemDiceAnim;

    public bool isRollingItem;

   public TextMeshProUGUI itemDiceName;

    public int result;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null) Instance = this;

        itemDiceAnimObj.SetActive(false);
        itemDice.SetActive(false);
        itemDiceName.enabled = false;
    }

    public IEnumerator RollDice()
    {
        itemDiceAnimObj.SetActive(true);
        itemDiceAnim.SetBool("isRolling", true);

        isRollingItem = true;

        int diceRoll;
        diceRoll = Random.Range(1, 7);

        yield return new WaitForSeconds(2.5f);

        itemDiceAnimObj.SetActive(false);
        itemDiceAnim.SetBool("isRolling", false);

        itemDice.SetActive(true);

        switch (diceRoll)
        {
            case 1:
                itemDicePivot.Rotate(0, 90, 0);
                break;

            case 2:
                itemDicePivot.Rotate(0, 90, -90);
                break;

            case 3:
                itemDicePivot.Rotate(0, 180, -90);
                break;

            case 4:
                itemDicePivot.Rotate(0, 360, -90);
                break;

            case 5:
                itemDicePivot.Rotate(0, 270, -90);
                break;

            case 6:
                itemDicePivot.Rotate(0, 270, 0);
                break;

            default:
                break;
        }

        yield return new WaitForSeconds(2f);

        BoardManager.instance.ActivateItem(diceRoll, itemDice);
    }
}