using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public int numberOfGames;

    public void OnPlayClicked()
    {
        int randomGame = Random.Range(0, numberOfGames);

        // Loads minigame scene accounting for menus.
        SceneManager.LoadScene(randomGame + 2);
    }
}
