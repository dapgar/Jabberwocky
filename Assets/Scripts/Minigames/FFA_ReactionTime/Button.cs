using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    private IState _currentState;

    [SerializeField]
    private IState _inactive;

    [SerializeField]
    private IState _ready;

    [SerializeField]
    private IState _pressed;

    public void SetInactive()
    {
        ChangeState(_inactive);
    }

    public void SetReady()
    {
        ChangeState(_ready);
    }

    public void SetPressed()
    {
        ChangeState(_pressed);
    }

    private void ChangeState(IState newState)
    {
        _currentState.Exit();
        _currentState = newState;
        _currentState.Enter();
    }
}
