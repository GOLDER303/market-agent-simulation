using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private WaypointPath path;

    [SerializeField]
    private float speed = 5f;

    private Vector3 currentTargetPoint;
    private int currentTargetPointIndex = 0;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        currentTargetPoint = path.GetControlPoint(0);
    }

    void Update()
    {
        Vector2 targetXZ = new(currentTargetPoint.x, currentTargetPoint.z);
        Vector2 positionXZ = new(transform.position.x, transform.position.z);

        float distance = Vector2.Distance(targetXZ, positionXZ);

        Debug.Log(distance);

        if (distance <= .1f)
        {
            Debug.Log("target changed");
            currentTargetPoint = path.GetControlPoint(++currentTargetPointIndex);
        }

        Vector3 direction = currentTargetPoint - transform.position;
        direction.y = 0;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = lookRotation;

        rb.linearVelocity = direction.normalized * speed;
    }
}
