using UnityEngine;

public class FollowingPathState : BaseCustomerState
{
    private readonly IntersectionsInterface intersectionsInterface;
    private IntersectionsInterface.Intersection currentTargetIntersection;
    private IntersectionsInterface.Intersection previousIntersection;

    public FollowingPathState(
        StateMachine<CustomerStateMachine.CustomerState> stateMachine,
        Customer customer,
        IntersectionsInterface intersectionsInterface
    )
        : base(stateMachine, customer)
    {
        this.intersectionsInterface = intersectionsInterface;
    }

    public override void EnterState()
    {
        Debug.Log("FollowingPath entered");

        if (intersectionsInterface != null)
        {
            if (currentTargetIntersection == null)
            {
                currentTargetIntersection = intersectionsInterface.GetNearestIntersection(
                    customer.transform.position
                );
            }

            Debug.Log(
                $"Initial target intersection: [{currentTargetIntersection.row}, {currentTargetIntersection.col}]"
            );
        }
        else
        {
            Debug.LogError("IntersectionsInterface not assigned to FollowingPathState");
        }
    }

    public override void ExitState()
    {
        rb.linearVelocity = Vector3.zero;
    }

    public override void Tick()
    {
        if (intersectionsInterface == null || currentTargetIntersection == null)
            return;

        Vector2 targetXZ =
            new(currentTargetIntersection.position.x, currentTargetIntersection.position.z);
        Vector2 positionXZ = new(customer.transform.position.x, customer.transform.position.z);

        float distance = Vector2.Distance(targetXZ, positionXZ);

        if (distance <= .1f)
        {
            ChooseNextIntersection();
        }

        MoveTowardsPosition(currentTargetIntersection.position);
    }

    private void ChooseNextIntersection()
    {
        var nextIntersection = intersectionsInterface.GetRandomStraightLineNeighbor(
            currentTargetIntersection,
            previousIntersection
        );

        if (nextIntersection != null)
        {
            previousIntersection = currentTargetIntersection;
            currentTargetIntersection = nextIntersection;

            Debug.Log(
                $"Moving to intersection: [{currentTargetIntersection.row}, {currentTargetIntersection.col}]"
            );
        }
    }

    public override void FixedTick() { }
}
