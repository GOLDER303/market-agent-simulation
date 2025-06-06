using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public Product CurrentTargetProduct { get; private set; }
    public float PickupRange => pickupRange;
    public float Speed => speed;
    public ShoppingListManager ShoppingList => shoppingList;
    public CustomerStateMachine StateMachine => stateMachine;
    public float CheckoutTime => checkoutTime;

    [SerializeField]
    private float pickupRange = .7f;

    [SerializeField]
    private CustomerStateMachine stateMachine;

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float checkoutTime = 2;

    [SerializeField]
    private ShoppingListManager shoppingList;

    private SightSensor sightSensor;

    private Animator animator;

    private void Awake()
    {
        sightSensor = GetComponentInChildren<SightSensor>();

        animator = GetComponentInChildren<Animator>();
        Debug.Log($"Customer {name} initialized with speed: {speed}, pickup range: {pickupRange}");
        animator.SetFloat("WalkSpeedMultiplier", speed);
    }

    void Update()
    {
        Product product = GetProductFromListInSight();

        // Switch to product targeting when a needed product is spotted while following path
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
        // Search through all visible objects for products on the shopping list
        foreach (GameObject objectInSight in sightSensor.ObjectsInSight)
        {
            if (objectInSight == null)
            {
                continue;
            }

            Product product = objectInSight.GetComponent<Product>();

            // Return first product found that matches shopping list
            if (product != null && shoppingList.ContainsProduct(product.ProductType))
            {
                return product;
            }
        }

        return null;
    }
}
