using UnityEngine;

public class Product : MonoBehaviour
{
    public ProductSO ProductSO
    {
        get => productSO;
    }

    [SerializeField]
    private ProductSO productSO;

    public ProductSO PickUp()
    {
        Destroy(gameObject);
        return productSO;
    }
}
