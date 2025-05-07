using UnityEngine;

public class BoardSquare
{
    public Vector2Int position;
    public GameObject visual;
    private Color originalColor;

    public BoardSquare(Vector2Int position, GameObject visual)
    {
        this.position = position;
        this.visual = visual;

        Renderer renderer = visual.GetComponent<Renderer>();
        if (renderer != null)
        {
            
            originalColor = renderer.material.color;
        }
    }

    public void SetColor(Color color)
    {
        Renderer renderer = visual.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = color;
        }
    }

    public void ResetColor()
    {
        SetColor(originalColor);
    }
}



