using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private int gridWidth = 4;
    [SerializeField] private int gridHeight = 4;
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private float moveSpeed = 2f;
    private Vector2 currentGridPosition;
    private Vector3 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        currentGridPosition = new Vector2(gridWidth / 2, gridHeight / 2);
        transform.position = new Vector3(
            currentGridPosition.x * cellSize,
            transform.position.y,
            currentGridPosition.y * cellSize
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
        Vector2 input = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            input = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            input = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            input = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            input = Vector2.right;
        }

        if (input != Vector2.zero)
        {
            Vector2 newGridPosition = currentGridPosition + input;
            if (newGridPosition.x >= 0 && newGridPosition.x < gridWidth &&
                newGridPosition.y >= 0 && newGridPosition.y < gridHeight)
            {
                currentGridPosition = newGridPosition;
                targetPosition = new Vector3(
                    currentGridPosition.x * cellSize,
                    transform.position.y,
                    currentGridPosition.y * cellSize
                );
                isMoving = true;
                animator.SetFloat("Speed", 1);
                transform.rotation = Quaternion.LookRotation(new Vector3(input.x, 0, input.y));
            }
            else
            {
                animator.SetFloat("Speed", 0);
            }
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
    }

    void MoveToTarget()
    {
        Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        _rb.MovePosition(newPosition);

        if (Vector3.Distance(transform.position, targetPosition) <= 0.01f)
        {
            isMoving = false;
            animator.SetFloat("Speed", 0);
        }
    }
}
