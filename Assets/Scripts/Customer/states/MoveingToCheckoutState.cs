using UnityEngine;

public class MovingToCheckoutState : BaseCustomerState
{
    public MovingToCheckoutState(
        StateMachine<CustomerStateMachine.CustomerState> stateMachine,
        Customer customer
    )
        : base(stateMachine, customer) { }

    public override void EnterState() { }

    public override void ExitState() { }

    public override void Tick()
    {
        Object.Destroy(customer);
    }

    public override void FixedTick() { }
}
