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
    private WaypointPath path;

    private void Awake()
    {
        InitializeStates();
    }

    private void InitializeStates()
    {
        states.Add(CustomerState.FollowingPath, new FollowingPathState(this, customer, path));
        states.Add(CustomerState.MovingToProduct, new MovingToProductState(this, customer));
        states.Add(CustomerState.PickingUpProduct, new PickingUpProductState(this, customer));
        states.Add(CustomerState.MovingToCheckout, new MovingToProductState(this, customer));

        currentState = states[CustomerState.FollowingPath];
    }
}
