using System;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingListManager : MonoBehaviour
{
    [SerializeField]
    private readonly List<ProductType> productList = new();

    [SerializeField]
    private Transform UIOrigin;

    [SerializeField]
    private ProductDatabase productDatabase;

    [SerializeField]
    private float UISpacing = .3f;

    private HashSet<ProductType> productSet;
    private readonly HashSet<ProductType> pickedUpProducts = new();

    private readonly HashSet<GameObject> currentUIObjects = new();

    private void Awake()
    {
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

        UpdateUI();

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

        UpdateUI();
    }

    private void UpdateUI()
    {
        foreach (GameObject UIObject in currentUIObjects)
        {
            Destroy(UIObject);
        }

        currentUIObjects.Clear();

        int index = 0;

        foreach (ProductType productType in productSet)
        {
            ProductConfig productConfig = productDatabase.GetConfig(productType);

            Vector3 spawnPosition = UIOrigin.position;
            if (index > 0)
            {
                spawnPosition += (int)Math.Pow(-1, index) * UISpacing * Vector3.right;
            }

            GameObject product = Instantiate(
                productConfig.productPrefab,
                spawnPosition,
                Quaternion.identity,
                transform
            );

            currentUIObjects.Add(product);

            product.layer = 5;
            product.tag = "Untagged";

            index++;
        }
    }
}
