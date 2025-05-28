using UnityEngine;

[ExecuteInEditMode]
public class IntersectionsGenerator : MonoBehaviour
{
    public MapLayout layout;

    private void awake()
    {
        if (!layout)
        {
            layout = GetComponent<MapLayout>();
        }
        if (!layout)
        {
            Debug.LogError("MapLayout component is not assigned or found on the GameObject.");
            return;
        }

        // spawnIntersections();
    }


    private void OnDrawGizmos()
    {
        Vector3 mapSize = layout.GetMapSize();
        Vector3 origin = layout.GetGridOrigin();

        for (int row = 0; row <= layout.rows; row++)
        {
            for (int col = 0; col <= layout.columns; col++)
            {
                Vector3 intersecPos = origin + new Vector3(
                    layout.entranceAreaWidth
                        + layout.spacingColumns / 2
                        + col * (layout.shelvesPerSet * layout.shelfSize.x + layout.spacingColumns),
                    0,
                    layout.spacingRows / 2 + row * (2 * layout.shelfSize.z + layout.spacingRows)
                    );

                Gizmos.DrawWireSphere(intersecPos, .2f);
            }
        }
    }
}
