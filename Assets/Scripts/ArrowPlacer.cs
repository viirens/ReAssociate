using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ArrowData
{
    public Vector2Int gridPosition;
}

public class ArrowPlacer : MonoBehaviour
{
    public GridManager gridManager;
    public List<ArrowData> passThroughPoints = new List<ArrowData>();
    public LineRenderer lineRenderer;

    void Start()
    {
        PlaceArrow();
        Vector2Int playerPosition = gridManager.GetPlayerPosition();
        Debug.Log($"Player position: {playerPosition}");
    }


    void PlaceArrow()
    {
        // first point is the player position
        
        lineRenderer.positionCount = passThroughPoints.Count;
        Vector2Int playerPosition = gridManager.GetPlayerPosition();
        // set pass through point 0 to player position
        // passThroughPoints.Insert(0, new ArrowData { gridPosition = playerPosition });
        passThroughPoints[0].gridPosition = playerPosition;

        // lineRenderer.SetPosition(0, new Vector3(playerPosition.x, 0.6f, playerPosition.y));
        for (int i = 0; i < passThroughPoints.Count; i++)
        {
            Debug.Log("i: " + i);
            Debug.Log("Current point: " + passThroughPoints[i].gridPosition);
            lineRenderer.SetPosition(i, new Vector3(passThroughPoints[i].gridPosition.x, 0.6f, passThroughPoints[i].gridPosition.y));
        }
    }

    void UpdateArrow()
    {
        for (int i = 0; i < passThroughPoints.Count; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(passThroughPoints[i].gridPosition.x, 0.6f, passThroughPoints[i].gridPosition.y));
        }
    }

    public void MoveArrowPointsWithPlayer(Vector2Int direction)
    {
        Debug.Log("moving in direction: " + direction);
        Debug.Log("Current second point: " + passThroughPoints[0].gridPosition);
        Vector2Int playerPosition = gridManager.GetPlayerPosition();
        Debug.Log($"Updated player position: {playerPosition}");

        passThroughPoints[0].gridPosition = playerPosition;
        for (int i = 1; i < passThroughPoints.Count; i++)
        {
            passThroughPoints[i].gridPosition += direction;
            Debug.Log("Current point: " + passThroughPoints[i].gridPosition);
        }

        Debug.Log("pass through points: " + passThroughPoints);
        UpdateArrow();
    }
}
