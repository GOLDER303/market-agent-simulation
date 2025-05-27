using UnityEngine;

public class FloorGenerator : MonoBehaviour
{
    public MapLayout layout;

    private GameObject floorPlane;

    void Awake()
    {
        if (layout == null)
        {
            Debug.LogWarning("FloorGenerator: MapLayout not assigned.");
            return;
        }

        CreateOrUpdateFloor();
    }

    void CreateOrUpdateFloor()
    {
        if (floorPlane == null)
        {
            floorPlane = GameObject.Find("FloorPlane");
            if (floorPlane == null)
            {
                floorPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                floorPlane.name = "FloorPlane";
                floorPlane.transform.parent = this.transform;
            }
        }

        Vector3 size = layout.GetMapSize();
        Vector3 origin = layout.GetGridOrigin();

        floorPlane.transform.localScale = new Vector3(size.x / 10f, 1f, size.z / 10f);
        floorPlane.transform.position = origin + new Vector3(size.x / 2f, 0f, size.z / 2f);
    }
}
