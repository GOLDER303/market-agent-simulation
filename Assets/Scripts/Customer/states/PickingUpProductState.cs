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

        ProductSO pickedUpProductSO = customer.CurrentTargetProduct.PickUp();
        int productsLeftCount = customer.ShoppingList.RemoveProductFromList(
            pickedUpProductSO.productName
        );

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
