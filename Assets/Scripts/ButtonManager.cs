using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    // Loads Loading Screen
    public void ToLoadScene()
    {
        SceneManager.LoadScene(1);
    }
}
