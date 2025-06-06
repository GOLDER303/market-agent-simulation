using UnityEngine;

public class PickingUpProductState : BaseCustomerState
{
    public PickingUpProductState(
        StateMachine<CustomerStateMachine.CustomerState> stateMachine,
        Customer customer
    )
        : base(stateMachine, customer) { }

    public override void EnterState()
    {
        navMeshAgent.SetDestination(customer.transform.position);
        animator.SetBool("isPickingUp", true);
    }

    public override void ExitState()
    {
        animator.SetBool("isPickingUp", false);
    }

    public override void Tick()
    {
        if (customer.CurrentTargetProduct == null)
        {
            return;
        }

        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (animatorStateInfo.normalizedTime < 1)
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
