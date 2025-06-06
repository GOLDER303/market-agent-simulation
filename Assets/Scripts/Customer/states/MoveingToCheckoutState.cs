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
        // Check if customer has reached their current destination
        if (navMeshAgent.remainingDistance < .1f)
        {
            if (hasCheckedOut)
            {
                // Customer has finished checkout and reached exit - remove from simulation
                CustomerSpawner.DecreaseCustomerNumber();
                Object.Destroy(customer.gameObject);
            }
            else
            {
                // Customer at checkout counter - simulate checkout process with timer
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
