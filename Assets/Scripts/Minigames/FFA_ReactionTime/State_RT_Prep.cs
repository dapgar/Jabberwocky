using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_RT_Prep : MonoBehaviour, IState
{
    public void OnEnter()
    {
        //roundStartTimer.Reset();
        //playersLeft = players.Length;
        //round++;

        //playerScored = null;
        //for (int i = 0; i < buttonPressed.Length; i++)
        //{
        //    buttonPressed[i] = false;
        //    pbScript[i].SetInactive();
        //}

        //timerText.gameObject.SetActive(false);
    }

    public void Execute()
    {
        //roundStartTimer.Countdown();
        //timerText.text = (Mathf.Ceil(roundStartTimer.time)).ToString("F0");
    }

    public void OnExit()
    {
        throw new System.NotImplementedException();
    }
}
