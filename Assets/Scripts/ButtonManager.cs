using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    // Loads Loading Screen
    public void ToLoadScene()
    {
        SceneChanger.Instance.ChangeScene(1);
    }
}
