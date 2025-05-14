using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProductDatabase", menuName = "Data/Product Database")]
public class ProductDatabase : ScriptableObject
{
    public List<ProductConfig> products;

    public ProductConfig GetConfig(ProductType type)
    {
        return products.Find(p => p.type == type);
    }
}