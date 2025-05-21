using UnityEngine;

public class Product : MonoBehaviour
{
    public ProductSO ProductSO
    {
        get => productSO;
    }

    [SerializeField]
    private ProductSO productSO;

    public void PickUp()
    {
        Destroy(gameObject);
    }
}
