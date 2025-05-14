using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnProductInfo
{
    public ProductType type;
    public int count = 1;
}

public class ResourceSpawner : MonoBehaviour
{
    [Header("References")]
    public MapBounds mapBounds;
    public ProductDatabase database;
    public List<SpawnProductInfo> spawnList;

    void Start()
    {
        if (database == null || mapBounds == null)
        {
            Debug.LogError("No references in ResourceSpawner");
            return;
        }

        SpawnShelves();
    }

    void SpawnShelves()
    {
        foreach (var item in spawnList)
        {
            var cfg = database.GetConfig(item.type);
            if (cfg == null) continue;

            for (int i = 0; i < item.count; i++)
            {
                Vector2 rnd = mapBounds.GetRandomPoint();
                Vector3 pos = new Vector3(rnd.x, 0, rnd.y);

                var shelfGO = Instantiate(cfg.shelfPrefab, pos, Quaternion.identity, transform);
                shelfGO.name = $"{item.type}_Shelf_{i}";
                var shelf = shelfGO.GetComponent<Shelf>();
                shelf.productType = item.type;
                shelf.SetColor(cfg.shelfColor);

                var anchor = shelfGO.transform.Find("ProductAnchor") ?? shelfGO.transform;
                var prodGO = Instantiate(cfg.productPrefab, anchor);
                prodGO.transform.localPosition = Vector3.zero;
                prodGO.transform.localRotation = Quaternion.identity;
                prodGO.name = $"{item.type}_Model_{i}";
            }
        }
    }
}
