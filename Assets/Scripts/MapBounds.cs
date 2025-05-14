using UnityEngine;

public class MapBounds : MonoBehaviour
{
    [Header("map bounds")]
    public float width = 30f;
    public float height = 30f;

    // Metoda pomocnicza do losowania pozycji
    public Vector2 GetRandomPoint()
    {
        float x = Random.Range(-width / 2f, width / 2f);
        float y = Random.Range(-height / 2f, height / 2f);
        return new Vector2(x, y);
    }

    // Dla wizualizacji w edytorze
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, 1f, height));
    }
}
