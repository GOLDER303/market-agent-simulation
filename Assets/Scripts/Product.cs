using UnityEngine;

public class Product : MonoBehaviour
{
    public ProductType ProductType
    {
        get => productType;
    }

    [SerializeField]
    private ProductType productType;

    public ProductType PickUp()
    {
        Destroy(gameObject);
        return productType;
    }
}
