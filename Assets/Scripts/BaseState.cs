using System;

public abstract class BaseState<EState>
    where EState : Enum
{
    public EState key;
    protected readonly StateMachine<EState> stateMachine;

    public BaseState(StateMachine<EState> stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public abstract void EnterState();
    public abstract void ExitState();

    public virtual void Tick() { }

    public virtual void FixedTick() { }
}
