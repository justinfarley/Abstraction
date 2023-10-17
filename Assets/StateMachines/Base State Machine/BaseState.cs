using UnityEngine;
using System;
public abstract class BaseState<TState> where TState : Enum
{
    public TState StateKey { get; private set; }
    public BaseState(TState key)
    {
        StateKey = key;
    }
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract TState GetNextState();
}
