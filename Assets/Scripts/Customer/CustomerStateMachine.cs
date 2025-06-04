using UnityEngine;

public class CustomerStateMachine : StateMachine<CustomerStateMachine.CustomerState>
{
    public enum CustomerState
    {
        FollowingPath,
        MovingToProduct,
        PickingUpProduct,
        MovingToCheckout
    }

    [SerializeField]
    private Customer customer;

    [SerializeField]
    private IntersectionsInterface intersectionsInterface;

    private Vector3 checkoutPosition;
    private Vector3 exitPosition;

    public void Initialize(Vector3 checkoutPosition, Vector3 exitPosition)
    {
        this.checkoutPosition = checkoutPosition;
        this.exitPosition = exitPosition;

        InitializeStates();
        enabled = true;
    }

    private void InitializeStates()
    {
        intersectionsInterface ??= FindFirstObjectByType<IntersectionsInterface>();
        if (intersectionsInterface == null)
        {
            Debug.LogError("IntersectionsInterface not found in scene!");
        }

        states.Add(
            CustomerState.FollowingPath,
            new FollowingPathState(this, customer, intersectionsInterface)
        );
        states.Add(CustomerState.MovingToProduct, new MovingToProductState(this, customer));
        states.Add(CustomerState.PickingUpProduct, new PickingUpProductState(this, customer));
        states.Add(
            CustomerState.MovingToCheckout,
            new MovingToCheckoutState(this, customer, checkoutPosition, exitPosition)
        );

        currentState = states[CustomerState.FollowingPath];
    }
}
