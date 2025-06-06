using System.Collections.Generic;
using UnityEngine;

public class ShelfSpawner : MonoBehaviour
{
    public MapLayout layout;

    [Header("Shelf Prefab")]
    public GameObject shelfPrefab;

    [Header("Product Database")]
    public ProductDatabase productDatabase;

    [Header("Spawned Shelves (Read Only)")]
    public List<GameObject> spawnedShelves = new List<GameObject>();

    public void GenerateShelves()
    {
        SpawnShelves();
        AssignRandomProductsToShelves();
    }

    [ContextMenu("Spawn Shelves")]
    public void SpawnShelves()
    {
        ClearShelves();

        if (
            layout == null
            || shelfPrefab == null
            || productDatabase == null
            || productDatabase.products == null
            || productDatabase.products.Count == 0
        )
        {
            Debug.LogWarning(
                "ShelfSpawner: MapLayout, shelfPrefab or productDatabase not assigned."
            );
            return;
        }

        // Calculate dimensions for shelf sets
        float setWidth = layout.shelvesPerSet * layout.shelfSize.x;
        float rowDepth = 2 * layout.shelfSize.z;
        Vector3 origin = layout.GetGridOrigin();

        // Generate shelves in a grid pattern: rows and columns of shelf sets
        for (int row = 0; row < layout.rows; row++)
        {
            for (int col = 0; col < layout.columns; col++)
            {
                // Calculate position for each shelf set with proper spacing
                Vector3 setOrigin =
                    origin
                    + new Vector3(
                        layout.entranceAreaWidth
                            + layout.spacingColumns
                            + col * (setWidth + layout.spacingColumns),
                        0,
                        layout.spacingRows + row * (rowDepth + layout.spacingRows)
                    );

                // Create individual shelves within each set (back-to-back arrangement)
                for (int s = 0; s < layout.shelvesPerSet; s++)
                {
                    Vector3 shelfPos =
                        setOrigin
                        + new Vector3(
                            s * layout.shelfSize.x + layout.shelfSize.x / 2f,
                            layout.shelfSize.y / 2f,
                            layout.shelfSize.z / 2f
                        );
                    // Front-facing shelf
                    GameObject shelfObj = Instantiate(
                        shelfPrefab,
                        shelfPos + new Vector3(0, 0, layout.shelfSize.z),
                        Quaternion.identity,
                        this.transform
                    );
                    spawnedShelves.Add(shelfObj);
                    // Back-facing shelf (mirrored)
                    GameObject shelfObjMirrored = Instantiate(
                        shelfPrefab,
                        shelfPos,
                        Quaternion.Euler(0, 180, 0),
                        this.transform
                    );
                    spawnedShelves.Add(shelfObjMirrored);
                }
            }
        }
    }

    private void AssignRandomProductsToShelves()
    {
        foreach (var shelfObj in spawnedShelves)
        {
            // Randomly assign a product type to each shelf
            var config = productDatabase.products[Random.Range(0, productDatabase.products.Count)];
            Shelf shelf = shelfObj.GetComponent<Shelf>();
            if (shelf != null)
            {
                shelf.productType = config.type;
                // shelf.SetColor(config.shelfColor);
            }

            // Populate shelf with product instances at anchor points
            if (config.productPrefab != null)
            {
                Transform anchorsParent = shelfObj.transform.Find("Anchors");
                foreach (Transform anchor in anchorsParent)
                {
                    GameObject product = Instantiate(
                        config.productPrefab,
                        anchor.position,
                        Quaternion.identity,
                        anchor
                    );

                    // Align product bottom with anchor position for proper placement
                    Renderer renderer = product.GetComponentInChildren<Renderer>();
                    if (renderer != null)
                    {
                        float modelBottomY = renderer.bounds.min.y;
                        float anchorY = anchor.position.y;
                        float deltaY = anchorY - modelBottomY;
                        product.transform.position += Vector3.up * deltaY;
                    }

                    product.transform.localRotation = Quaternion.identity;
                }
            }
        }
    }

    [ContextMenu("Clear Shelves")]
    public void ClearShelves()
    {
        for (int i = spawnedShelves.Count - 1; i >= 0; i--)
        {
            if (spawnedShelves[i] != null)
            {
                Destroy(spawnedShelves[i]);
            }
        }
        spawnedShelves.Clear();
    }
}
