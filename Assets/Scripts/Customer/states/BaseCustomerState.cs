using UnityEngine;

public abstract class BaseCustomerState : BaseState<CustomerStateMachine.CustomerState>
{
    protected Customer customer;
    protected Rigidbody rb;

    protected BaseCustomerState(
        StateMachine<CustomerStateMachine.CustomerState> stateMachine,
        Customer customer
    )
        : base(stateMachine)
    {
        this.customer = customer;
        rb = customer.GetComponent<Rigidbody>();
    }

    protected void MoveTowardsPosition(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - customer.transform.position;
        direction.y = 0;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        customer.transform.rotation = lookRotation;

        rb.linearVelocity = direction.normalized * customer.Speed;
    }
}
