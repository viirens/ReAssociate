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
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        gridManager.PlaceItemAt(currentGridPosition, gameObject);
        arrowPlacer.MoveArrowPointsWithPlayer(input);
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
}
