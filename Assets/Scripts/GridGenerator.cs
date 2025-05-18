using UnityEngine;

[ExecuteInEditMode]
public class GridGenerator : MonoBehaviour
{
    public MapLayout layout;
    public Color boundsColor = Color.green;
    public Color gridColor = Color.gray;
    public Color entranceColor = Color.cyan;

    private void OnDrawGizmos()
    {
        if (layout == null) return;

        Vector3 mapSize = layout.GetMapSize();
        Vector3 origin = layout.GetGridOrigin();

        Gizmos.color = boundsColor;
        Gizmos.DrawWireCube(origin + new Vector3(mapSize.x / 2f, 0, mapSize.z / 2f), mapSize);

        Gizmos.color = gridColor;

        float setWidth = layout.shelvesPerSet * layout.shelfSize.x;
        float rowDepth = 2 * layout.shelfSize.z;

        for (int row = 0; row < layout.rows; row++)
        {
            for (int col = 0; col < layout.columns; col++)
            {
                Vector3 cellOrigin = origin + new Vector3(
                    layout.entranceAreaWidth + layout.spacingColumns + col * (setWidth + layout.spacingColumns),
                    0,
                    layout.spacingRows + row * (rowDepth + layout.spacingRows)
                );

                Vector3 cellSize = new Vector3(setWidth, 0, rowDepth);
                Gizmos.DrawWireCube(cellOrigin + new Vector3(cellSize.x / 2f, 0, cellSize.z / 2f), cellSize);
            }
        }

        Gizmos.color = entranceColor;
        Vector3 entranceZoneSize = new Vector3(layout.entranceAreaWidth, 0, mapSize.z);
        Gizmos.DrawWireCube(origin + new Vector3(entranceZoneSize.x / 2f, 0, entranceZoneSize.z / 2f), entranceZoneSize);
    }
}
