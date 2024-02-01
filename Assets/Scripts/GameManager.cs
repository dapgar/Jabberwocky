using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<StoneScript> players;

    public TextMeshProUGUI diceTutText;
    public TextMeshProUGUI rollText;
    public Image diceFace;
    public List<Sprite> diceFaceImages;

    public DiceRollManager drm;

    public bool canStartNextTurn = true;

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

        rollText.enabled = false;
        diceFace.enabled = false;
        diceTutText.enabled = true;
        diceTutText.text = "'Space' to Roll Dice";
        drm = GameObject.Find("_DiceRollManager").GetComponent<DiceRollManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Rolls Dice
        if (Input.GetKeyDown(KeyCode.Space) && !drm.isRolling && canStartNextTurn)
        {
            StartCoroutine(RollAndMove());
        }
    }

    IEnumerator RollAndMove()
    {
        canStartNextTurn = false;

        // UI
        diceTutText.enabled = false;

        // Dice Logic
        drm.RollDice();
        yield return new WaitForSeconds(4f);

        int result = drm.DetectResult();
        players[0].steps = result;

        // Movement Logic
        if (players[0].routePos + players[0].steps < players[0].currentRoute.childNodeList.Count)
        {
            // UI
            rollText.enabled = true;
            diceFace.enabled = true;
            diceFace.sprite = diceFaceImages[result - 1];

            StartCoroutine(players[0].Move());
        }
        else
        {
            Debug.Log("Rolled Number Is Too High");
            canStartNextTurn = true;
        }
    }
}
