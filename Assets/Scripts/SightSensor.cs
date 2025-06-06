using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SightSensor : MonoBehaviour
{
    public List<GameObject> ObjectsInSight
    {
        get => objectsInSight;
    }

    [SerializeField]
    private float distance = 12f;

    [SerializeField]
    private float angle = 60f;

    [SerializeField]
    private float height = 2f;

    [SerializeField]
    private Color meshColor = Color.white;

    [SerializeField]
    private int scanFrequency = 30;

    [SerializeField]
    private LayerMask layers;

    [SerializeField]
    private LayerMask occlusionLayers;

    private readonly Collider[] colliders = new Collider[500];
    private float scanInterval;
    private float scanTimer;
    private int collidersInSphereCount;
    private readonly List<GameObject> objectsInSight = new();

    private Mesh mesh;

    void Start()
    {
        scanInterval = 1f / scanFrequency;
    }

    void Update()
    {
        scanTimer -= Time.deltaTime;

        if (scanTimer <= 0)
        {
            scanTimer += scanInterval;
            Scan();
        }
    }

    private void Scan()
    {
        // Use non-allocating sphere overlap to find potential targets within sight distance
        collidersInSphereCount = Physics.OverlapSphereNonAlloc(
            transform.position,
            distance,
            colliders,
            layers,
            QueryTriggerInteraction.Collide
        );

        objectsInSight.Clear();

        // Check each found collider against sight constraints (angle, height, occlusion)
        for (int i = 0; i < collidersInSphereCount; i++)
        {
            GameObject gameObject = colliders[i].gameObject;
            if (IsInSight(gameObject))
            {
                objectsInSight.Add(gameObject);
            }
        }
    }

    private bool IsInSight(GameObject gameObject)
    {
        Vector3 origin = transform.position;
        Vector3 destination = gameObject.transform.position;
        Vector3 direction = destination - origin;

        // Check if target is within vertical sight range
        if (direction.y < 0 || direction.y > height)
        {
            return false;
        }

        // Flatten direction to horizontal plane for angle calculation
        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);

        // Check if target is within sight cone angle
        if (deltaAngle > angle)
        {
            return false;
        }

        // Perform line-of-sight check for occlusion at eye level
        origin.y += height / 2;
        destination.y = origin.y;

        if (Physics.Linecast(origin, destination, occlusionLayers))
        {
            return false;
        }

        return true;
    }

    private Mesh CreateMesh()
    {
        Mesh mesh = new();

        // Calculate mesh complexity based on segments for sight cone visualization
        int segments = 10;
        int trianglesCount = (segments * 4) + 2 + 2;
        int verticesCount = trianglesCount * 3;

        Vector3[] vertices = new Vector3[verticesCount];
        int[] triangles = new int[verticesCount];

        // Define key points of the sight cone (bottom and top)
        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;

        int vertexIndex = 0;

        vertices[vertexIndex++] = bottomCenter;
        vertices[vertexIndex++] = bottomLeft;
        vertices[vertexIndex++] = topLeft;

        vertices[vertexIndex++] = topLeft;
        vertices[vertexIndex++] = topCenter;
        vertices[vertexIndex++] = bottomCenter;

        vertices[vertexIndex++] = bottomCenter;
        vertices[vertexIndex++] = topCenter;
        vertices[vertexIndex++] = topRight;

        vertices[vertexIndex++] = topRight;
        vertices[vertexIndex++] = bottomRight;
        vertices[vertexIndex++] = bottomCenter;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;

        // Generate curved surface of the sight cone using segments
        for (int i = 0; i < segments; i++)
        {
            bottomRight =
                Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;

            topRight = bottomRight + Vector3.up * height;
            topLeft = bottomLeft + Vector3.up * height;

            // Create front face triangles
            vertices[vertexIndex++] = bottomLeft;
            vertices[vertexIndex++] = bottomRight;
            vertices[vertexIndex++] = topRight;

            vertices[vertexIndex++] = topRight;
            vertices[vertexIndex++] = topLeft;
            vertices[vertexIndex++] = bottomLeft;

            // Create top and bottom cap triangles
            vertices[vertexIndex++] = topCenter;
            vertices[vertexIndex++] = topLeft;
            vertices[vertexIndex++] = topRight;

            vertices[vertexIndex++] = bottomCenter;
            vertices[vertexIndex++] = bottomRight;
            vertices[vertexIndex++] = bottomLeft;

            currentAngle += deltaAngle;
        }

        for (int i = 0; i < verticesCount; i++)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnValidate()
    {
        mesh = CreateMesh();
        scanInterval = 1f / scanFrequency;
    }

    private void OnDrawGizmos()
    {
        if (mesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, distance);
        for (int i = 0; i < collidersInSphereCount; i++)
        {
            if (colliders[i] != null)
            {
                Gizmos.DrawWireSphere(colliders[i].transform.position, .2f);
            }
        }

        Gizmos.color = Color.green;

        foreach (var gameObject in objectsInSight)
        {
            if (gameObject != null)
            {
                Gizmos.DrawSphere(gameObject.transform.position, .2f);
            }
        }
    }
}
