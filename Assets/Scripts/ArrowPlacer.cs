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
    }


    void PlaceArrow()
    {
        lineRenderer.positionCount = passThroughPoints.Count;
        for (int i = 0; i < passThroughPoints.Count; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(passThroughPoints[i].gridPosition.x, 0.6f, passThroughPoints[i].gridPosition.y));
        }
    }
}