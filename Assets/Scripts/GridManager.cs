using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridWidth = 4;
    public int gridHeight = 4;
    public float cellSize = 1f;
    public GameObject gridTilePrefab;

    private GridCell[,] gridCells;

    void Start()
    {
        gridCells = new GridCell[gridWidth, gridHeight];
        GenerateGrid();
        // SetPlayerPosition();
        // Debug.Log($"Player position: {currentPlayerPosition}");
    }

    void Update()
    {
        UpdateTileColor();
    }

    // if a cell is visited change the color of the tile
    void UpdateTileColor()
    {
        // loop through all cells and change the color of the tile if the cell is visited
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                if (gridCells[x, z].alreadyVisited)
                {
                    gridCells[x, z].tileGameObject.GetComponent<Renderer>().material.color = Color.red;
                }
            }
        }
    }

    void GenerateGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                Vector3 position = new Vector3(x * cellSize, 0.01f, z * cellSize);
                GameObject tile = Instantiate(gridTilePrefab, position, Quaternion.identity, transform);

                gridCells[x, z] = new GridCell
                {
                    position = new Vector2Int(x, z),
                    tileGameObject = tile,
                    item = null,
                    isOccupied = false
                };
            }
        }
    }

    public GridCell GetGridCell(Vector2Int position)
    {
        if (IsPositionValid(position))
        {
            return gridCells[position.x, position.y];
        }
        return null;
    }

    public void PlaceItemAt(Vector2Int position, GameObject item)
    {
        GridCell cell = GetGridCell(position);
        if (cell != null && !cell.isOccupied)
        {
            cell.item = item;
            cell.isOccupied = true;
        }
    }

    public void RemoveItemAt(Vector2Int position)
    {
        GridCell cell = GetGridCell(position);
        Debug.Log("remove item at: " + position);
        // cell.alreadyVisited = true;
        if (cell != null && cell.isOccupied)
        {
            cell.item = null;
            cell.isOccupied = false;
        }
    }

    public bool IsPositionValid(Vector2Int position)
    {
        return position.x >= 0 && position.x < gridWidth &&
               position.y >= 0 && position.y < gridHeight;
    }

    public Vector2Int GetPlayerPosition()
    {
        // loop through all cells and find the one that has the player
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                if (gridCells[x, z].item != null && gridCells[x, z].item.name == "Player(Clone)")
                {
                    // Debug.Log($"Player found at {x}, {z}");
                    return new Vector2Int(x, z);
                }
            }
        }
        return new Vector2Int(-1, -1);
    }

    public bool PointIsBeyondGrid(Vector2Int point)
    {
        return point.x < 0 || point.x >= gridWidth || point.y < 0 || point.y >= gridHeight;
    }
}

public class GridCell
{
    public Vector2Int position;
    public GameObject tileGameObject;
    public GameObject item;
    public bool isOccupied;
    public bool alreadyVisited;
}
