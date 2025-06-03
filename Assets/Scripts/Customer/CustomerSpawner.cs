using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField]
    private Customer customerPrefab;

    [SerializeField]
    private float spawnInterval = 5f;

    [SerializeField]
    private int maxCustomers = 5;

    private int currentCustomers = 0;
    private float timer = 0f;

    private void Awake()
    {
        timer = spawnInterval;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval && currentCustomers <= maxCustomers)
        {
            SpawnCustomer();
            timer = 0f;
        }
    }

    private void SpawnCustomer()
    {
        Customer customer = Instantiate(customerPrefab, transform.position, Quaternion.identity);
        customer.gameObject.name = "Customer_" + currentCustomers;

        customer.ShoppingList.Randomize(3);
        customer.StateMachine.Initialize();

        currentCustomers++;
    }

    public void DecreaseCustomerNumber()
    {
        currentCustomers--;
    }
}
