using UnityEngine;

public class MovingToProductState : BaseCustomerState
{
    private Product targetProduct;

    public MovingToProductState(
        StateMachine<CustomerStateMachine.CustomerState> stateMachine,
        Customer customer
    )
        : base(stateMachine, customer) { }

    public override void EnterState()
    {
        targetProduct = customer.CurrentTargetProduct;

        if (targetProduct == null)
        {
            stateMachine.ChangeState(CustomerStateMachine.CustomerState.FollowingPath);
        }
    }

    public override void ExitState()
    {
        targetProduct = null;
        rb.linearVelocity = Vector3.zero;
    }

    public override void Tick()
    {
        if (targetProduct == null)
        {
            stateMachine.ChangeState(CustomerStateMachine.CustomerState.FollowingPath);
            return;
        }

        MoveTowardsPosition(targetProduct.transform.position);

        Vector2 positionXZ = new(customer.transform.position.x, customer.transform.position.z);
        Vector2 targetXZ =
            new(targetProduct.transform.position.x, targetProduct.transform.position.z);

        float distanceToTargetProduct = Vector2.Distance(targetXZ, positionXZ);

        if (distanceToTargetProduct <= customer.PickupRange)
        {
            //TODO: move pickup logic to PickingUpProductState
            targetProduct.PickUp();
            stateMachine.ChangeState(CustomerStateMachine.CustomerState.FollowingPath);
        }
    }
}
