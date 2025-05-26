using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public Product CurrentTargetProduct { get; private set; }
    public float PickupRange => pickupRange;
    public float Speed => speed;

    [SerializeField]
    private float pickupRange = .7f;

    [SerializeField]
    private CustomerStateMachine stateMachine;

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private List<string> productList = new();

    private HashSet<string> productSet;
    private SightSensor sightSensor;

    void Start()
    {
        sightSensor = GetComponentInChildren<SightSensor>();

        productSet = new HashSet<string>(productList);
    }

    void Update()
    {
        Product product = GetProductFromListInSight();

        if (
            CurrentTargetProduct == null
            && product != null
            && stateMachine.CurrentStateKey == CustomerStateMachine.CustomerState.FollowingPath
        )
        {
            CurrentTargetProduct = product;
            stateMachine.ChangeState(CustomerStateMachine.CustomerState.MovingToProduct);
        }
    }

    private Product GetProductFromListInSight()
    {
        foreach (GameObject objectInSight in sightSensor.ObjectsInSight)
        {
            if (objectInSight == null)
            {
                continue;
            }

            Product product = objectInSight.GetComponent<Product>();

            if (product != null && productSet.Contains(product.ProductSO.productName))
            {
                return product;
            }
        }

        return null;
    }
}
