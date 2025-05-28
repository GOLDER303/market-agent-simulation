using UnityEngine;

public class PickingUpProductState : BaseCustomerState
{
    public PickingUpProductState(
        StateMachine<CustomerStateMachine.CustomerState> stateMachine,
        Customer customer
    )
        : base(stateMachine, customer) { }

    public override void EnterState() { }

    public override void ExitState() { }

    public override void Tick()
    {
        if (customer.CurrentTargetProduct == null)
        {
            return;
        }

        ProductType pickedUpProductType = customer.CurrentTargetProduct.PickUp();
        int productsLeftCount = customer.ShoppingList.RemoveProductFromList(pickedUpProductType);

        if (productsLeftCount > 0)
        {
            stateMachine.ChangeState(CustomerStateMachine.CustomerState.FollowingPath);
        }
        else
        {
            stateMachine.ChangeState(CustomerStateMachine.CustomerState.MovingToCheckout);
        }
    }
}
