using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [SerializeField]
    private WaypointPath path;

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private List<string> productList = new();

    private HashSet<string> productSet;
    private Vector3 currentTargetPoint;
    private int currentTargetPointIndex = 0;
    private Rigidbody rb;
    private SightSensor sightSensor;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sightSensor = GetComponentInChildren<SightSensor>();

        productSet = new HashSet<string>(productList);

        currentTargetPoint = path.GetControlPoint(0);
    }

    void Update()
    {
        Product product = GetProductFromListInSight();

        if (product != null)
        {
            MoveTowardsPosition(product.transform.position);
        }
        else
        {
            MoveAlongWaypointPath();
        }
    }

    private void MoveAlongWaypointPath()
    {
        Vector2 targetXZ = new(currentTargetPoint.x, currentTargetPoint.z);
        Vector2 positionXZ = new(transform.position.x, transform.position.z);

        float distance = Vector2.Distance(targetXZ, positionXZ);

        if (distance <= .1f)
        {
            currentTargetPoint = path.GetControlPoint(++currentTargetPointIndex);
        }

        MoveTowardsPosition(currentTargetPoint);
    }

    private void MoveTowardsPosition(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = lookRotation;

        rb.linearVelocity = direction.normalized * speed;
    }

    private Product GetProductFromListInSight()
    {
        foreach (GameObject objectInSight in sightSensor.ObjectsInSight)
        {
            Product product = objectInSight.GetComponent<Product>();

            if (product != null && productSet.Contains(product.ProductSO.name))
            {
                return product;
            }
        }

        return null;
    }
}
