using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConfigurationManager : MonoBehaviour
{
    private List<PlayerConfiguration> playerConfigs;

    [SerializeField]
    private int maxPlayers = 4;

    [SerializeField]
    private GameObject[] playerCharacters;

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
        }
    }

    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return playerConfigs;
    }

    public void SetPlayerChar(int index, GameObject charPrefab)
    {
        playerConfigs[index].PlayerChar = charPrefab;
    }

    public void SetPlayerHat(int index, GameObject hatPrefab)
    {
        
    }

    public void ReadyPlayer(int index)
    {
        //Debug.Log("Player Ready Index #" + index);
        playerConfigs[index].IsReady = true;

        // check to see if all the players have joined the game and if they are ready
        if (playerConfigs.Count == maxPlayers && playerConfigs.All(p => p.IsReady == true))
        {
            if (GameManager.instance) GameManager.instance.SetNumPlayers(maxPlayers);
            // TEMP CHANGE LATER
            Debug.Log("All Players Are Ready!");
            //SceneChanger.Instance.ChangeScene(7);
        }
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
            SetPlayerChar(playerInput.playerIndex, playerCharacters[playerInput.playerIndex]);
            ReadyPlayer(playerInput.playerIndex);
        }
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
}