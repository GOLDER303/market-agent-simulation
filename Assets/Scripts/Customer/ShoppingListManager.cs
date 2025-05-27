using System.Collections.Generic;
using UnityEngine;

public class ShoppingListManager
{
    private readonly List<string> productList = new();

    private readonly HashSet<string> productSet;
    private readonly HashSet<string> pickedUpProducts = new();

    public ShoppingListManager(List<string> productList)
    {
        this.productList = productList;
        productSet = new HashSet<string>(productList);
    }

    public bool ContainsProduct(string productName)
    {
        return productSet.Contains(productName);
    }

    public int RemoveProductFromList(string productName)
    {
        if (!ContainsProduct(productName))
        {
            return productSet.Count;
        }

        productSet.Remove(productName);
        pickedUpProducts.Add(productName);

        return productSet.Count;
    }
}
