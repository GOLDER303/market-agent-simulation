using UnityEngine;

public class EntranceExitSpawner : MonoBehaviour
{
    public MapLayout layout;
    public GameObject entrancePrefab;
    public GameObject exitPrefab;

    void Start()
    {
        SpawnEntranceAndExit();
    }

    public void SpawnEntranceAndExit()
    {
        if (layout == null || entrancePrefab == null || exitPrefab == null)
        {
            Debug.LogWarning("EntranceExitSpawner: Missing references.");
            return;
        }

        Vector3 origin = layout.GetGridOrigin();

        Vector3 entranceSize = GetPrefabSize(entrancePrefab);
        Vector3 entrancePos = origin + new Vector3(0, entranceSize.y / 2, entranceSize.z);
        Instantiate(entrancePrefab, entrancePos, Quaternion.identity, this.transform);

        Vector3 exitSize = GetPrefabSize(exitPrefab);
        Vector3 exitPos =
            origin + new Vector3(0, exitSize.y / 2, layout.GetMapSize().z - exitSize.z);
        Instantiate(exitPrefab, exitPos, Quaternion.identity, this.transform);
    }

    private Vector3 GetPrefabSize(GameObject prefab)
    {
        if (prefab == null)
            return new Vector3();
        Renderer r = prefab.GetComponentInChildren<Renderer>();
        if (r == null)
            return new Vector3();
        return r.bounds.size;
    }
}
