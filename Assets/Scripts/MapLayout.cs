using UnityEngine;

[ExecuteInEditMode]
public class MapLayout : MonoBehaviour
{
    [Header("Grid Layout")]
    public int rows = 4;
    public int columns = 6;
    public int shelvesPerSet = 2;

    [Header("Shelf Prefab")]
    public GameObject shelfPrefab;

    [Header("Spacing")]
    public float spacingRows = 2f;
    public float spacingColumns = 2f;

    [Header("Entrance/Exit Area")]
    public float entranceAreaWidth = 8f;

    public Vector3 shelfSize = new Vector3(1, 1, 1); // default fallback

    private void OnValidate()
    {
        if (shelfPrefab != null)
        {
            Renderer r = shelfPrefab.GetComponentInChildren<Renderer>();
            if (r != null)
                shelfSize = r.bounds.size;
        }
    }

    public Vector3 GetMapSize()
    {
        float width =
            entranceAreaWidth
            + 2 * spacingColumns
            + columns * (shelvesPerSet * shelfSize.x)
            + (columns - 1) * spacingColumns;

        float depth = 2 * spacingRows + rows * (2 * shelfSize.z) + (rows - 1) * spacingRows;

        return new Vector3(width, 0f, depth);
    }

    public Vector3 GetGridOrigin()
    {
        Vector3 origin = transform.position;
        return origin;
    }
}
