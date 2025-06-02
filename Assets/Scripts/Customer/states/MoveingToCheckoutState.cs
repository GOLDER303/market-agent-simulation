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
        Debug.LogWarning("customer Destroy");
        Object.Destroy(customer.gameObject);
    }

    public override void FixedTick() { }
}
