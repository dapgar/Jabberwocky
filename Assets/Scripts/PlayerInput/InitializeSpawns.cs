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

    public UnityEvent onPlayersSpawned;

    // Start is called before the first frame update
    void Start()
    {
        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();
        for (int i = 0; i < playerConfigs.Length; i++)
        {
            InitializePlayer(playerConfigs[i], i);
            //var player = Instantiate(playerPrefab, playerSpawns[i].position, playerSpawns[i].rotation, gameObject.transform);
            //player.GetComponent<PlayerInputHandler>().InitializePlayer(playerConfigs[i]);
        }
    }

    public void InitializePlayer(PlayerConfiguration pConfig, int index)
    {
        var player = Instantiate(playerPrefab, playerSpawns[index].position, playerSpawns[index].rotation, gameObject.transform);
        player.GetComponent<PlayerInputHandler>().InitializePlayer(pConfig);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
