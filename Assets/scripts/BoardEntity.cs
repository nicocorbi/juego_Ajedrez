using UnityEngine;

public class BoardEntity
{
    public Vector2Int Position { get; private set; }
    public GameObject Instance { get; private set; }

    public BoardEntity(Vector2Int initialPosition, GameObject prefab)
    {
        Position = initialPosition;
        Instance = GameObject.Instantiate(prefab, new Vector3(initialPosition.x, 0, initialPosition.y), Quaternion.identity);
    }

    public void MoveTo(Vector2Int newPosition)
    {
        Position = newPosition;
        Instance.transform.position = new Vector3(newPosition.x, 0, newPosition.y);
    }
}

