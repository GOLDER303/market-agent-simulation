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
        if (intersectionsInterface != null)
        {
            if (currentTargetIntersection == null)
            {
                currentTargetIntersection = intersectionsInterface.GetNearestIntersection(
                    customer.transform.position
                );
            }

            navMeshAgent.SetDestination(currentTargetIntersection.position);
        }
        else
        {
            Debug.LogError("IntersectionsInterface not assigned to FollowingPathState");
        }
    }

    public override void ExitState() { }

    public override void Tick()
    {
        if (intersectionsInterface == null || currentTargetIntersection == null)
        {
            return;
        }

        Vector2 targetXZ =
            new(currentTargetIntersection.position.x, currentTargetIntersection.position.z);
        Vector2 positionXZ = new(customer.transform.position.x, customer.transform.position.z);

        float distance = Vector2.Distance(targetXZ, positionXZ);

        float tolerance = Mathf.Max(0.1f, navMeshAgent.velocity.magnitude * Time.deltaTime * 1.5f);

        if (distance <= tolerance)
        {
            ChooseNextIntersection();
            navMeshAgent.SetDestination(currentTargetIntersection.position);
        }
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
        }
    }

    public override void FixedTick() { }
}
