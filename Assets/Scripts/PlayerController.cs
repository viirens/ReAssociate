using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float moveSpeed = 2f;

    private GridManager gridManager;
    private Vector2Int currentGridPosition;
    private Vector3 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();

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

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            input = new Vector2Int(0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            input = new Vector2Int(0, -1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            input = new Vector2Int(-1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            input = new Vector2Int(1, 0);
        }

        if (input != Vector2Int.zero)
        {
            Vector2Int newGridPosition = currentGridPosition + input;

            if (newGridPosition.x >= 0 && newGridPosition.x < gridManager.gridWidth &&
                newGridPosition.y >= 0 && newGridPosition.y < gridManager.gridHeight)
            {
                currentGridPosition = newGridPosition;
                targetPosition = new Vector3(
                    currentGridPosition.x * gridManager.cellSize,
                    transform.position.y,
                    currentGridPosition.y * gridManager.cellSize
                );
                isMoving = true;
                animator.SetFloat("Speed", 1);

                Vector3 direction = new Vector3(input.x, 0, input.y);
                if (direction != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(direction);
                }
            }
            else
            {
                animator.SetFloat("Speed", 0);
            }
        }
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
