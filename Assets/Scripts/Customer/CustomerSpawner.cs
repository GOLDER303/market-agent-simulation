using TMPro;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField]
    private Customer customerPrefab;

    [SerializeField]
    private float spawnInterval = 5f;

    [SerializeField]
    private int maxCustomers = 5;

    [SerializeField]
    private Transform checkoutPosition;

    [SerializeField]
    private Transform exitPosition;

    [SerializeField]
    private TextMeshProUGUI customerCounterText;

    private static int currentCustomers = 0;
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

        UpdateUI();
    }

    private void SpawnCustomer()
    {
        Customer customer = Instantiate(customerPrefab, transform.position, Quaternion.identity);
        customer.gameObject.name = "Customer_" + currentCustomers;

        customer.ShoppingList.Randomize(3);
        customer.StateMachine.Initialize(checkoutPosition.position, exitPosition.position);

        currentCustomers++;
    }

    public static void DecreaseCustomerNumber()
    {
        currentCustomers--;
    }

    private void UpdateUI()
    {
        customerCounterText.text = "Customers: " + currentCustomers;
    }
}
