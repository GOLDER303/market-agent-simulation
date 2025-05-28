using System.Collections.Generic;
using UnityEngine;

public class ShoppingListManager
{
    private readonly List<ProductType> productList = new();

    private readonly HashSet<ProductType> productSet;
    private readonly HashSet<ProductType> pickedUpProducts = new();

    public ShoppingListManager(List<ProductType> productList)
    {
        this.productList = productList;
        productSet = new HashSet<ProductType>(productList);
    }

    public bool ContainsProduct(ProductType productName)
    {
        return productSet.Contains(productName);
    }

    public int RemoveProductFromList(ProductType productType)
    {
        if (!ContainsProduct(productType))
        {
            return productSet.Count;
        }

        productSet.Remove(productType);
        pickedUpProducts.Add(productType);

        return productSet.Count;
    }
}
