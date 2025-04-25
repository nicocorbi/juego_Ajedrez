using UnityEngine;

public class boardSquare 
{
    public Vector2Int position;
    public BoardEntity boardEntity;
    public boardSquare(Vector2Int position, GameObject squarePrefab)
    {
        linkedEntity = null;
    }
    

}
public class MainManagerPruebas : MonoBehaviour
{
    [SerializeField] GameObject entityPrefab;
    [SerializeField] int initialEntityAmount = 2;
    int boardWidth = 5, boardHeight = 5;
    BoardSquare[,] board;
    BoardEntityPruebas[] entities;
    int currentEntityIndex = 1;
    private void Awake()
    {
        Vector3[] vectors = new Vector3[200];
        board = new BoardSquare[boardWidth, boardHeigth];
        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeigth; j++)
            {
                //crear casilla
            }
        }
    }
}
