using System.IO;
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

    public WaypointPath path;

    public void Initialize()
    {
        InitializeStates();
    }

    private void InitializeStates()
    {
        states.Add(CustomerState.FollowingPath, new FollowingPathState(this, customer, path));
        states.Add(CustomerState.MovingToProduct, new MovingToProductState(this, customer));
        states.Add(CustomerState.PickingUpProduct, new PickingUpProductState(this, customer));
        states.Add(CustomerState.MovingToCheckout, new MovingToCheckoutState(this, customer));

        currentState = states[CustomerState.FollowingPath];
    }
}
