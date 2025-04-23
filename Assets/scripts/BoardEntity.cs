using UnityEngine;

public class BoardEntity 
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Vector2Int position;
    public BoardEntity()
    {
        
    }

    // Update is called once per frame
    public BoardEntity(Vector2Int initialPosition,GameObject prefab)
    {
        this.position = initialPosition;
    }
    public void Move(Vector2Int movement)
    {
        position += movement;
    }
}
