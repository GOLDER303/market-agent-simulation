using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public Product CurrentTargetProduct { get; private set; }
    public float PickupRange => pickupRange;
    public float Speed => speed;
    public ShoppingListManager ShoppingList => shoppingList;
    public CustomerStateMachine StateMachine => stateMachine;

    [SerializeField]
    private float pickupRange = .7f;

    [SerializeField]
    private CustomerStateMachine stateMachine;

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private List<ProductType> shoppingProductsList;

    private ShoppingListManager shoppingList;

    private SightSensor sightSensor;

    private void Awake()
    {
        shoppingList = new(shoppingProductsList);
        sightSensor = GetComponentInChildren<SightSensor>();
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

            if (product != null && shoppingList.ContainsProduct(product.ProductType))
            {
                return product;
            }
        }

        return null;
    }
}
