using UnityEngine;

public class MainManager : MonoBehaviour
{
    public GameObject entityPrefab;
    public int initialEntityAmount = 1;

    private BoardEntity[] entities;

    private void Awake()
    {
        entities = new BoardEntity[initialEntityAmount];

        for (int i = 0; i < initialEntityAmount; i++)
        {
            entities[i] = new BoardEntity(new Vector2Int(i, 0), entityPrefab);
        }
    }

    private void Update()
    {
        // Ejemplo de movimiento con teclas
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            entities[0].MoveTo(entities[0].Position + Vector2Int.right);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            entities[0].MoveTo(entities[0].Position + Vector2Int.left);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            entities[0].MoveTo(entities[0].Position + Vector2Int.up);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            entities[0].MoveTo(entities[0].Position + Vector2Int.down);
        }
    }
}
