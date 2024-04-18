using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerConfigurationManager : MonoBehaviour
{
    private List<PlayerConfiguration> playerConfigs;

    [SerializeField]
    private int maxPlayers = 4;

    [SerializeField]
    private GameObject[] playerCharacters;
    private bool[] charPrefabUsed;
    [SerializeField]
    private Sprite[] charIcons;
    [SerializeField]
    private Sprite nullIcon;

    [SerializeField]
    private GameObject[] playerHats;

    public static PlayerConfigurationManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Trying to create another singleton.");
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            playerConfigs = new List<PlayerConfiguration>();
            charPrefabUsed = new bool[playerCharacters.Length];
        }
    }

    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return playerConfigs;
    }

    public GameObject[] GetPlayerPrefabs()
    {
        return playerCharacters;
    }
    public bool CheckCharPrefabUsed(int index)
    {
        return charPrefabUsed[index];
    }

    public Sprite[] GetUsedPlayerIcons()
    {
        Sprite[] usedIcons = new Sprite[maxPlayers];
        int currentPlayers = playerConfigs.Count;

        for (int i = 0; i < currentPlayers; i++)
        {
            usedIcons[i] = charIcons[playerConfigs[i].CharPrefabIndex];
        }

        if (currentPlayers < maxPlayers)
        {
            for (int i = currentPlayers; i < maxPlayers; i++)
            {
                //usedIcons[i] = charIcons[NextAvailableCharIndex()];
                usedIcons[i] = nullIcon;
            }
        }

        return usedIcons;
    }

    public void SetPlayerChar(int index, int charPrefabIndex)
    {
        playerConfigs[index].PlayerChar = playerCharacters[charPrefabIndex];
        playerConfigs[index].CharPrefabIndex = charPrefabIndex;
        charPrefabUsed[charPrefabIndex] = true;
    }

    public void SetPlayerHat(int index, GameObject hatPrefab)
    {
        playerConfigs[index].PlayerHat = hatPrefab;
    }

    public void ReadyPlayer(int index)
    {
        //Debug.Log("Player Ready Index #" + index);
        playerConfigs[index].IsReady = true;

        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            return;
        }
        if (playerConfigs.Count == maxPlayers && playerConfigs.All(p => p.IsReady == true))
        {
            if (GameManager.instance) GameManager.instance.SetNumPlayers(maxPlayers);
            // TEMP CHANGE LATER
            Debug.Log("All Players Are Ready!");
            SceneChanger.Instance.ChangeScene(1);
        }

        StartCoroutine(AllReadyCheck());
    }

    IEnumerator AllReadyCheck()
    {
        Debug.Log("Start Check");
        yield return new WaitForSeconds(3);
        // check to see if all the players have joined the game and if they are ready
        if (/*playerConfigs.Count == maxPlayers && */playerConfigs.All(p => p.IsReady == true))
        {
            if (GameManager.instance) GameManager.instance.SetNumPlayers(maxPlayers);
            // TEMP CHANGE LATER
            Debug.Log("All Players Are Ready!");
            SceneChanger.Instance.ChangeScene(1);
        }
        Debug.Log("Finish Check");
    }

    public void HandlePlayerJoin(PlayerInput playerInput)
    {
        // check to see if this input is being used already
        if(!playerConfigs.Any(p => p.PlayerIndex == playerInput.playerIndex))
        {
            Debug.Log("Player Joined Index #" + playerInput.playerIndex);

            playerInput.transform.SetParent(transform);
            playerConfigs.Add(new PlayerConfiguration(playerInput));

            // TEMP CHANGE LATER
            //SetPlayerChar(playerInput.playerIndex, playerInput.playerIndex);
            //SetPlayerHat(playerInput.playerIndex, playerCharacters[playerInput.playerIndex]);
            //ReadyPlayer(playerInput.playerIndex);

            // for testing purposes in other scenes
            if (SceneManager.GetActiveScene().name != "MainMenu")
            {
                int charIndex = NextAvailableCharIndex();
                SetPlayerChar(playerInput.playerIndex, charIndex);
                //SetPlayerHat(playerInput.playerIndex, playerCharacters[playerInput.playerIndex]);
                ReadyPlayer(playerInput.playerIndex);
                GameObject.FindAnyObjectByType<InitializeSpawns>()?.InitializePlayer(playerConfigs[playerInput.playerIndex], playerInput.playerIndex);
            }
            //GameObject.FindAnyObjectByType<InitializeSpawns>()?.InitializePlayer(playerConfigs[playerInput.playerIndex], playerInput.playerIndex);
        }
    }

    // THIS IS TEMP AND THIS CODE IS TECHNICALLY ELSEWHERE
    private int NextAvailableCharIndex()
    {
        int i = 0;
        for (i = 0; i < playerCharacters.Length; i++)
        {
            if (!charPrefabUsed[i])
            {
                break;
            }
        }

        return i;
    }
}

public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput playerInput)
    {
        PlayerIndex = playerInput.playerIndex;
        Input = playerInput;
    }
    public PlayerInput Input { get; set; }
    public int PlayerIndex { get; set; }
    public bool IsReady { get; set; }
    public GameObject PlayerChar { get; set; }
    public int CharPrefabIndex { get; set; }
    public GameObject PlayerHat { get; set; }
}