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

        navMeshAgent.SetDestination(targetProduct.transform.position);
    }

    public override void ExitState()
    {
        targetProduct = null;
    }

    public override void Tick()
    {
        if (targetProduct == null)
        {
            return;
        }

        // Calculate horizontal distance to target (ignoring Y-axis)
        Vector2 positionXZ = new(customer.transform.position.x, customer.transform.position.z);
        Vector2 targetXZ =
            new(targetProduct.transform.position.x, targetProduct.transform.position.z);

        float distanceToTargetProduct = Vector2.Distance(targetXZ, positionXZ);

        // Switch to pickup state when close enough to the product
        if (distanceToTargetProduct <= customer.PickupRange)
        {
            stateMachine.ChangeState(CustomerStateMachine.CustomerState.PickingUpProduct);
        }
    }
}
