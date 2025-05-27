using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class StateMachine<EState> : MonoBehaviour
    where EState : Enum
{
    public EState CurrentStateKey => currentStateKey;

    protected Dictionary<EState, BaseState<EState>> states = new();
    protected BaseState<EState> currentState;
    protected EState currentStateKey;

    private bool isTransitioning = false;

    private void Start()
    {
        currentStateKey ??= states.Keys.First();
        currentState ??= states[currentStateKey];
        currentState.EnterState();
    }

    private void Update()
    {
        if (!isTransitioning)
        {
            currentState.Tick();
        }
    }

    private void FixedUpdate()
    {
        if (!isTransitioning)
        {
            currentState.FixedTick();
        }
    }

    public void ChangeState(EState nextStateKey)
    {
        if (isTransitioning || nextStateKey.Equals(currentStateKey))
        {
            return;
        }

        isTransitioning = true;
        currentState.ExitState();
        currentState = states[nextStateKey];
        currentStateKey = nextStateKey;
        currentState.EnterState();
        isTransitioning = false;
    }
}
