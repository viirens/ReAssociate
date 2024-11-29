using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ItemData
{
    public GameObject itemPrefab;
    public Vector2Int gridPosition;
}

public class ItemPlacer : MonoBehaviour
{
    public GridManager gridManager;
    public GameObject playerPrefab;
    public Vector2Int playerStartPosition;
    public List<ItemData> itemsToPlace = new List<ItemData>();

    void Start()
    {
        PlacePlayer();
        PlaceItems();
    }

    void PlacePlayer()
    {
        if (playerStartPosition.x >= 0 && playerStartPosition.x < gridManager.gridWidth &&
            playerStartPosition.y >= 0 && playerStartPosition.y < gridManager.gridHeight)
        {
            Vector3 playerPosition = new Vector3(
                playerStartPosition.x * gridManager.cellSize,
                0,
                playerStartPosition.y * gridManager.cellSize
            );
            Instantiate(playerPrefab, playerPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogError($"Player start position is out of grid bounds: {playerStartPosition}");
        }
    }

    void PlaceItems()
    {
        foreach (ItemData itemData in itemsToPlace)
        {
            if (itemData.gridPosition.x >= 0 && itemData.gridPosition.x < gridManager.gridWidth &&
                itemData.gridPosition.y >= 0 && itemData.gridPosition.y < gridManager.gridHeight)
            {
                Vector3 itemPosition = new Vector3(
                    itemData.gridPosition.x * gridManager.cellSize,
                    0,
                    itemData.gridPosition.y * gridManager.cellSize
                );
                Instantiate(itemData.itemPrefab, itemPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning($"Item '{itemData.itemPrefab.name}' has a grid position out of bounds: {itemData.gridPosition}");
            }
        }
    }
}