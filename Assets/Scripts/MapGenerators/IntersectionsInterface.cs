using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class IntersectionsInterface : MonoBehaviour
{
    [System.Serializable]
    public class Intersection
    {
        public Vector3 position;
        public int row;
        public int col;

        public Intersection(Vector3 pos, int r, int c)
        {
            position = pos;
            row = r;
            col = c;
        }
    }

    public MapLayout layout;

    private Intersection[,] intersections;
    private int gridRows;
    private int gridCols;
    public int GridRows => gridRows;
    public int GridCols => gridCols;

    [ContextMenu("Regenerate Intersections")]
    public void RegenerateIntersections()
    {
        GenerateIntersections();
    }

    private void OnDrawGizmos()
    {
        if (intersections == null)
            return;

        for (int row = 0; row < gridRows; row++)
        {
            for (int col = 0; col < gridCols; col++)
            {
                Gizmos.DrawWireSphere(intersections[row, col].position, .2f);
            }
        }
    }

    public void GenerateIntersections()
    {
        if (!layout)
            return;

        Vector3 origin = layout.GetGridOrigin();

        // Create intersection grid one larger than shelf grid to cover all pathways
        gridRows = layout.rows + 1;
        gridCols = layout.columns + 1;

        intersections = new Intersection[gridRows, gridCols];

        // Generate intersections at pathway crossings between shelf sets
        for (int row = 0; row < gridRows; row++)
        for (int col = 0; col < gridCols; col++)
        {
            Vector3 intersectionPos =
                origin
                + new Vector3(
                    layout.entranceAreaWidth
                        + layout.spacingColumns / 2
                        + col * (layout.shelvesPerSet * layout.shelfSize.x + layout.spacingColumns),
                    0,
                    layout.spacingRows / 2 + row * (2 * layout.shelfSize.z + layout.spacingRows)
                );

            intersections[row, col] = new Intersection(intersectionPos, row, col);
        }
    }

    public Intersection GetNearestIntersection(Vector3 position)
    {
        float minDistance = float.MaxValue;
        Intersection nearest = null;

        for (int row = 0; row < gridRows; row++)
        {
            for (int col = 0; col < gridCols; col++)
            {
                float distance = Vector3.Distance(position, intersections[row, col].position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = intersections[row, col];
                }
            }
        }

        return nearest;
    }

    public List<Intersection> GetStraightLineNeighbors(
        Intersection current,
        Intersection previous = null
    )
    {
        List<Intersection> neighbors = new List<Intersection>();

        int row = current.row;
        int col = current.col;

        // Add all four cardinal direction neighbors (up, down, left, right)
        if (row > 0)
            neighbors.Add(intersections[row - 1, col]);

        if (row < gridRows - 1)
            neighbors.Add(intersections[row + 1, col]);

        if (col > 0)
            neighbors.Add(intersections[row, col - 1]);

        if (col < gridCols - 1)
            neighbors.Add(intersections[row, col + 1]);

        // Remove previous intersection to prevent immediate backtracking
        if (previous != null && neighbors.Count > 1)
        {
            neighbors.RemoveAll(n => n.row == previous.row && n.col == previous.col);
        }

        return neighbors;
    }

    public Intersection GetRandomStraightLineNeighbor(
        Intersection current,
        Intersection previous = null
    )
    {
        List<Intersection> neighbors = GetStraightLineNeighbors(current, previous);

        if (neighbors.Count == 0)
        {
            Debug.LogWarning("No straight line neighbors found for the current intersection.");
            return null;
        }

        int randomIndex = Random.Range(0, neighbors.Count);
        return neighbors[randomIndex];
    }

    public Intersection GetIntersection(int row, int col)
    {
        if (row >= 0 && row < gridRows && col >= 0 && col < gridCols)
            return intersections[row, col];
        return null;
    }
}
