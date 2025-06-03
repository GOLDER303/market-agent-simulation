using Unity.AI.Navigation;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private FloorGenerator floorGenerator;

    [SerializeField]
    private ShelfSpawner shelfSpawner;

    [SerializeField]
    private EntranceExitSpawner entranceExitSpawner;

    [SerializeField]
    private IntersectionsInterface intersectionsInterface;

    private void Awake()
    {
        shelfSpawner.GenerateShelves();
        floorGenerator.GenerateFloor();
        entranceExitSpawner.SpawnEntranceAndExit();
        intersectionsInterface.GenerateIntersections();

        GenerateNavMesh();
    }

    private void GenerateNavMesh()
    {
        NavMeshSurface[] navMeshSurfaces = FindObjectsByType<NavMeshSurface>(
            FindObjectsSortMode.None
        );

        for (int i = 0; i < navMeshSurfaces.Length; i++)
        {
            navMeshSurfaces[i].BuildNavMesh();
        }
    }
}
