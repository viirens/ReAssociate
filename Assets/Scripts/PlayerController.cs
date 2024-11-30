using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float moveSpeed = 2f;

    public ArrowPlacer arrowPlacer;
    private GridManager gridManager;
    private Vector2Int currentGridPosition;
    private Vector3 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        arrowPlacer = gridManager.GetComponent<ArrowPlacer>();

        currentGridPosition = new Vector2Int(
            Mathf.RoundToInt(transform.position.x / gridManager.cellSize),
            Mathf.RoundToInt(transform.position.z / gridManager.cellSize)
        );
        targetPosition = transform.position;
    }

    void Update()
    {
        if (!isMoving)
        {
            HandleInput();
        }
        else
        {
            MoveToTarget();
        }
    }

    void HandleInput()
    {
        Vector2Int input = Vector2Int.zero;

        if (Input.GetKeyDown(KeyCode.W))
        {
            input = new Vector2Int(0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            input = new Vector2Int(0, -1);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            input = new Vector2Int(-1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            input = new Vector2Int(1, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoveAlongArrow();
        }

        if (input != Vector2Int.zero)
        {
            Vector2Int newGridPosition = currentGridPosition + input;

            if (gridManager.IsPositionValid(newGridPosition))
            {
                GridCell targetCell = gridManager.GetGridCell(newGridPosition);

                if (targetCell.isOccupied)
                {
                    Debug.Log($"Cell at {newGridPosition} is occupied by {targetCell.item.name}");
                    // Implement interaction logic with the item here
                    // if (targetCell.item.name == "Goal(Clone)" && arrowPlacer.passThroughPoints.Count == 1)
                    // {
                    //     Debug.Log("Goal reached");
                    //     // destroy the goal and move to point
                    //     Destroy(targetCell.item);
                    //     // MoveToNewArrowPoint(newGridPosition);
                    // }
                }
                else
                {
                    MoveToNewPosition(newGridPosition, input);
                }
            }
            else
            {
                animator.SetFloat("Speed", 0);
            }
        }
    }

    void MoveToNewPosition(Vector2Int newGridPosition, Vector2Int input)
    {
        if (arrowPlacer.ArrowWillBeOutOfBounds(input, newGridPosition))
        {
            Debug.Log("Arrow is out of bounds");
            return;
        }
        gridManager.RemoveItemAt(currentGridPosition);
        currentGridPosition = newGridPosition;
        targetPosition = new Vector3(
            currentGridPosition.x * gridManager.cellSize,
            transform.position.y,
            currentGridPosition.y * gridManager.cellSize
        );

        animator.SetFloat("Speed", 1);
        isMoving = true;

        Vector3 direction = new Vector3(input.x, 0, input.y);
        Debug.Log("move playerdirection: " + direction);
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        gridManager.PlaceItemAt(currentGridPosition, gameObject);
        arrowPlacer.MoveArrowPointsWithPlayer(input, currentGridPosition);
    }

    void MoveToNewArrowPoint(Vector2Int newGridPosition)
    {
        gridManager.RemoveItemAt(currentGridPosition);
        Vector2Int difference = arrowPlacer.passThroughPoints[0].gridPosition - currentGridPosition;
        currentGridPosition = newGridPosition;
        targetPosition = new Vector3(
            currentGridPosition.x * gridManager.cellSize,
            transform.position.y,
            currentGridPosition.y * gridManager.cellSize
        );
        animator.SetFloat("Speed", 1);
        isMoving = true;

        
        Vector3 direction = new Vector3(difference.x, 0, difference.y);
        Debug.Log("move to arrow direction: " + direction);
        
        // make this rotation relative to the arrow
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
        gridManager.PlaceItemAt(currentGridPosition, gameObject);
    }

    void MoveToTarget()
    {
        Vector3 newPosition = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );
        _rb.MovePosition(newPosition);

        if (Vector3.Distance(transform.position, targetPosition) <= 0.01f)
        {
            isMoving = false;
            animator.SetFloat("Speed", 0);
        }
    }

    void MoveAlongArrow()
    {
        if (gridManager.GetGridCell(arrowPlacer.passThroughPoints[1].gridPosition).isOccupied && arrowPlacer.passThroughPoints.Count == 2)
        {
            if (gridManager.GetGridCell(arrowPlacer.passThroughPoints[1].gridPosition).item.name == "Goal(Clone)")
            {
                Destroy(gridManager.GetGridCell(arrowPlacer.passThroughPoints[1].gridPosition).item);
                arrowPlacer.ShortenArrow();
                MoveToNewArrowPoint(arrowPlacer.passThroughPoints[0].gridPosition);
                return;
            }
        }

        if (!gridManager.IsPositionValid(arrowPlacer.passThroughPoints[1].gridPosition) || gridManager.GetGridCell(arrowPlacer.passThroughPoints[1].gridPosition).isOccupied)
        {
            Debug.Log("New position is not valid");
            return;
        }
        
        arrowPlacer.ShortenArrow();
        // vector should be difference between current player position and new position
        Vector2Int input = arrowPlacer.passThroughPoints[0].gridPosition - currentGridPosition;
        MoveToNewArrowPoint(arrowPlacer.passThroughPoints[0].gridPosition);
    }
}
