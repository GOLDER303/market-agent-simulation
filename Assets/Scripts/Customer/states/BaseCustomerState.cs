using UnityEngine;
using UnityEngine.AI;

public abstract class BaseCustomerState : BaseState<CustomerStateMachine.CustomerState>
{
    protected Customer customer;
    protected NavMeshAgent navMeshAgent;
    protected Animator animator;

    protected BaseCustomerState(
        StateMachine<CustomerStateMachine.CustomerState> stateMachine,
        Customer customer
    )
        : base(stateMachine)
    {
        this.customer = customer;
        navMeshAgent = customer.GetComponent<NavMeshAgent>();
        animator = customer.GetComponentInChildren<Animator>();
    }
}
