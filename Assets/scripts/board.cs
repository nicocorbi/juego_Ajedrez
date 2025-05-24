using UnityEngine;

public class BoardSquare
{
    public Vector2Int position;
    public GameObject visual;
    private Color logicalColor; // Color lógico (blanco o negro)

    public BoardSquare(Vector2Int position, GameObject visual, Color logicalColor)
    {
        this.position = position;
        this.visual = visual;
        this.logicalColor = logicalColor;
    }

    public Color GetLogicalColor()
    {
        return logicalColor;
    }

    public void SetColorVisual(Color color)
    {
        if (visual != null)
            visual.GetComponent<Renderer>().material.color = color;
    }

    public void ResetColor()
    {
        SetColorVisual(logicalColor);
    }
}





