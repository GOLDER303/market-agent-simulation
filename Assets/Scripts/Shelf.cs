using UnityEngine;

public class Shelf : MonoBehaviour
{
    [Header("shelf configuration")]
    public ProductType productType;

    [Header("Look")]
    public Renderer shelfRenderer;

    /// <summary>
    /// Wywoluj po ustawieniu productType, capacity i koloru
    /// </summary>
    public void SetColor(Color c)
    {
        if (shelfRenderer == null) return;
        var mat = Instantiate(shelfRenderer.sharedMaterial);
        mat.color = c;
        shelfRenderer.material = mat;
    }
}