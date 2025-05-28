using System;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingListManager
{
    private readonly List<ProductType> productList = new();

    private HashSet<ProductType> productSet;
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

    public void Randomize(int productsCount)
    {
        ProductType[] values = (ProductType[])Enum.GetValues(typeof(ProductType));
        int total = values.Length;

        if (productsCount > total)
        {
            throw new ArgumentException("Requesting more values than exist in the enum.");
        }

        HashSet<ProductType> resultSet = new();
        System.Random rand = new();

        while (resultSet.Count < productsCount)
        {
            ProductType randomValue = values[rand.Next(total)];
            resultSet.Add(randomValue);
        }

        productSet = resultSet;
        pickedUpProducts.Clear();
    }
}
