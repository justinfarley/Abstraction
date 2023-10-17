using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class StateMachine<TState> : MonoBehaviour where TState : Enum
{


    protected Dictionary<Enum, BaseState<TState>> States = new Dictionary<Enum, BaseState<TState>>();

    protected BaseState<TState> CurrentState;

    private void Start()
    {
        CurrentState.EnterState();
    }
    private void Update()
    {
        TState nextStateKey = CurrentState.GetNextState();
        if (nextStateKey.Equals(CurrentState.StateKey))
        {
            CurrentState.UpdateState();
        }
        else
        {
            TransitionToState(nextStateKey);
        }
    }
    public void TransitionToState(TState stateKey)
    {
        CurrentState.ExitState();
        CurrentState = States[stateKey];
        CurrentState.EnterState();
    }
}
