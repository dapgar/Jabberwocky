using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<StoneScript> players;
    public TextMeshProUGUI diceText;
    public DiceRollManager drm;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!instance)
        {
            instance = this;
        }

        drm = GameObject.Find("_DiceRollManager").GetComponent<DiceRollManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Rolls Dice
        if (Input.GetKeyDown(KeyCode.Space) && !drm.isRolling)
        {
            StartCoroutine(RollAndMove());
        }

        diceText.text = "'Space' to Roll Dice";
    }

    IEnumerator RollAndMove()
    {
        // Dice Logic
        drm.RollDice();
        yield return new WaitForSeconds(4f);
        players[0].steps = drm.DetectResult();

        // Movement Logic
        if (players[0].routePos + players[0].steps < players[0].currentRoute.childNodeList.Count)
        {
            StartCoroutine(players[0].Move());
        }
        else
        {
            Debug.Log("Rolled Number Is Too High");
        }
    }
}
