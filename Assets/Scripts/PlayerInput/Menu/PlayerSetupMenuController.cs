using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSetupMenuController : MonoBehaviour, ICancelHandler
{
    private int playerIndex;
    private PlayerInput playerInput;

    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private GameObject readyPanel;
    [SerializeField]
    private GameObject menuPanel;
    [SerializeField]
    private Button readyButton;

    [SerializeField]
    private Display charDisplay;

    [SerializeField]
    private Sprite backgroundImage;

    private float baseIgnoreTime = .5f;
    private float ignoreInputTime;
    private bool inputEnabled;

    public void Start()
    {
        GameObject.Find("Background").GetComponent<Image>().sprite = backgroundImage;
        GameObject.Find("BeginText")?.SetActive(false);
        GameObject.Find("BeginButtonImage")?.SetActive(false);
    }

    public void SetPlayerIndex(int pi)
    {
        playerIndex = pi;
        titleText.SetText("P" + (pi + 1).ToString());
        ignoreInputTime = Time.time + baseIgnoreTime;
        charDisplay.SetDisplayIndex(pi);
    }
    public void SetInputHandler()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!inputEnabled && Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }
    }

    public void SetChar()
    {
        if (!inputEnabled)
        {
            return;
        }

        if (PlayerConfigurationManager.Instance.CheckCharPrefabUsed(charDisplay.GetDisplayIndex()))
        {
            charDisplay.DisplayNext();
            return;
        }

        
        PlayerConfigurationManager.Instance.SetPlayerChar(playerIndex, charDisplay.GetDisplayIndex());
        readyPanel.SetActive(true);
        readyButton.Select();
        menuPanel.SetActive(false);

        ReadyPlayer();
    }

    public void ReadyPlayer()
    {
        if (!inputEnabled)
        {
            return;
        }

        PlayerConfigurationManager.Instance.ReadyPlayer(playerIndex);
        readyPanel.gameObject.SetActive(false);
    }

    public void OnCancel(BaseEventData eventData)
    {
        Debug.Log(eventData.currentInputModule);
        Debug.Log(playerInput);
        if (eventData.currentInputModule == playerInput)
        {
            menuPanel.SetActive(true);
            readyPanel.SetActive(false);
        }
    }
}
