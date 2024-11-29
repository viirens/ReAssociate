  using UnityEngine;

  public class GridManager : MonoBehaviour
  {
      public int gridWidth = 4;
      public int gridHeight = 4;
      public float cellSize = 1f;
      public GameObject gridTilePrefab;

      void Start()
      {
          GenerateGrid();
      }

      void GenerateGrid()
      {
          for (int x = 0; x < gridWidth; x++)
          {
              for (int z = 0; z < gridHeight; z++)
              {
                  Vector3 position = new Vector3(x * cellSize, 0.01f, z * cellSize);
                  Instantiate(gridTilePrefab, position, Quaternion.identity, transform);
              }
          }
    }
}
