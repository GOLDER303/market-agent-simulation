using UnityEngine;

public class FollowingPathState : BaseCustomerState
{
    private readonly WaypointPath path;
    private Vector3 currentTargetPoint;
    private int currentTargetPointIndex = 0;

    public FollowingPathState(
        StateMachine<CustomerStateMachine.CustomerState> stateMachine,
        Customer customer,
        WaypointPath path
    )
        : base(stateMachine, customer)
    {
        this.path = path;
    }

    public override void EnterState()
    {
        Debug.Log("FollowingPath entered");
    }

    public override void ExitState()
    {
        rb.linearVelocity = Vector3.zero;
    }

    public override void Tick()
    {
        Vector2 targetXZ = new(currentTargetPoint.x, currentTargetPoint.z);
        Vector2 positionXZ = new(customer.transform.position.x, customer.transform.position.z);

        float distance = Vector2.Distance(targetXZ, positionXZ);

        if (distance <= .1f)
        {
            currentTargetPoint = path.GetControlPoint(++currentTargetPointIndex);
        }

        MoveTowardsPosition(currentTargetPoint);
    }

    public override void FixedTick() { }
}
