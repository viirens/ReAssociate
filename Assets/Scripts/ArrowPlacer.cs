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
        lineRenderer.positionCount = passThroughPoints.Count;
        Vector2Int playerPosition = gridManager.GetPlayerPosition();
        passThroughPoints[0].gridPosition = playerPosition;

        for (int i = 0; i < passThroughPoints.Count; i++)
        {
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

    public void MoveArrowPointsWithPlayer(Vector2Int direction, Vector2Int playerPosition)
    {
        passThroughPoints[0].gridPosition = playerPosition;
        for (int i = 1; i < passThroughPoints.Count; i++)
        {
            passThroughPoints[i].gridPosition += direction;
        }

        UpdateArrow();
    }

    public bool ArrowWillBeOutOfBounds(Vector2Int direction, Vector2Int playerPosition)
    {
        // copy passThroughPoints to tempPassThroughPoints
        List<ArrowData> tempPassThroughPoints = new List<ArrowData>();
        foreach (var arrow in passThroughPoints)
        {
            tempPassThroughPoints.Add(new ArrowData { gridPosition = arrow.gridPosition });
        }

        // move the first point to the player position  
        tempPassThroughPoints[0].gridPosition = playerPosition;
        // move the rest of the points
        for (int i = 1; i < tempPassThroughPoints.Count; i++)
        {
            tempPassThroughPoints[i].gridPosition += direction;
        }

        // check if any points are out of bounds
        for (int i = 0; i < tempPassThroughPoints.Count; i++)
        {
            if (gridManager.PointIsBeyondGrid(tempPassThroughPoints[i].gridPosition))
            {
                return true;
            }
        }
        return false;
    }

    // in this function arrows first point will become that of the first index of passThroughPoints
    // happens on space button press
    public void ShortenArrow()
    {
        
        passThroughPoints[0].gridPosition = passThroughPoints[1].gridPosition;
        passThroughPoints.RemoveAt(1);
        lineRenderer.positionCount = passThroughPoints.Count;
        UpdateArrow();
    }
}
