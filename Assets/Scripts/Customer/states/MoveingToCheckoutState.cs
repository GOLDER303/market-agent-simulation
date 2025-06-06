using UnityEngine;

public class MovingToCheckoutState : BaseCustomerState
{
    private Vector3 checkoutPosition;
    private Vector3 exitPosition;

    private bool hasCheckedOut = false;
    private float checkoutTimer = 0f;

    public MovingToCheckoutState(
        StateMachine<CustomerStateMachine.CustomerState> stateMachine,
        Customer customer,
        Vector3 checkoutPosition,
        Vector3 exitPosition
    )
        : base(stateMachine, customer)
    {
        this.checkoutPosition = checkoutPosition;
        this.exitPosition = exitPosition;
    }

    public override void EnterState()
    {
        navMeshAgent.SetDestination(checkoutPosition);
    }

    public override void ExitState() { }

    public override void Tick()
    {
        if (navMeshAgent.remainingDistance < .1f)
        {
            if (hasCheckedOut)
            {
                CustomerSpawner.DecreaseCustomerNumber();
                Object.Destroy(customer.gameObject);
            }
            else
            {
                checkoutTimer += Time.deltaTime;
                if (checkoutTimer >= customer.CheckoutTime)
                {
                    hasCheckedOut = true;
                    navMeshAgent.SetDestination(exitPosition);
                }
            }
        }
    }
}
