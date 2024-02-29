using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InitializeSpawns : MonoBehaviour
{
    [SerializeField]
    private Transform[] playerSpawns;
    [SerializeField]
    private GameObject playerPrefab;

    private List<GameObject> players;

    public List<GameObject> GetPlayers()
    {
        return players;
    }

    public UnityEvent onPlayersSpawned;

    // Start is called before the first frame update
    void Start()
    {
        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();
        for (int i = 0; i < playerConfigs.Length; i++)
        {
            var player = Instantiate(playerPrefab, playerSpawns[i].position, playerSpawns[i].rotation, gameObject.transform);
            player.GetComponent<PlayerInputHandler>().InitializePlayer(playerConfigs[i]);

            players.Add(player);
        }

        // once all the players have spawned
        onPlayersSpawned?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
