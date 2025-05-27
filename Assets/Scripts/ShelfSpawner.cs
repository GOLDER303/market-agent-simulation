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

    void Start()
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

        float setWidth = layout.shelvesPerSet * layout.shelfSize.x;
        float rowDepth = 2 * layout.shelfSize.z;
        Vector3 origin = layout.GetGridOrigin();

        for (int row = 0; row < layout.rows; row++)
        {
            for (int col = 0; col < layout.columns; col++)
            {
                Vector3 setOrigin =
                    origin
                    + new Vector3(
                        layout.entranceAreaWidth
                            + layout.spacingColumns
                            + col * (setWidth + layout.spacingColumns),
                        0,
                        layout.spacingRows + row * (rowDepth + layout.spacingRows)
                    );

                for (int s = 0; s < layout.shelvesPerSet; s++)
                {
                    Vector3 shelfPos =
                        setOrigin
                        + new Vector3(
                            s * layout.shelfSize.x + layout.shelfSize.x / 2f,
                            layout.shelfSize.y / 2f,
                            layout.shelfSize.z / 2f
                        );
                    GameObject shelfObj = Instantiate(
                        shelfPrefab,
                        shelfPos + new Vector3(0, 0, layout.shelfSize.z),
                        Quaternion.identity,
                        this.transform
                    );
                    spawnedShelves.Add(shelfObj);
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
            var config = productDatabase.products[Random.Range(0, productDatabase.products.Count)];
            Shelf shelf = shelfObj.GetComponent<Shelf>();
            if (shelf != null)
            {
                shelf.productType = config.type;
                // shelf.SetColor(config.shelfColor);
            }

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
