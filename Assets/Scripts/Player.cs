using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    private float playerRadius = 0.7f;
    private float playerHeight = 2f;
    private bool isWalking;
    private Vector3 lastInteractDirection;


    private void Start()
    {
        _gameInput.OnInteract += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        Vector2 input = _gameInput.GetMovementVectorNormalized();     
        Vector3 moveDir = new Vector3(input.x, 0, input.y);
        if (moveDir != Vector3.zero)
        {
            lastInteractDirection = moveDir;
        }
        float interactionDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit hit, interactionDistance,countersLayerMask))
        {
            if(hit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                clearCounter.Interact();
            }
        }
    }


    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    private void HandleInteractions()
    {
        Vector2 input = _gameInput.GetMovementVectorNormalized();     
        Vector3 moveDir = new Vector3(input.x, 0, input.y);
        if (moveDir != Vector3.zero)
        {
            lastInteractDirection = moveDir;
        }
        float interactionDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit hit, interactionDistance,countersLayerMask))
        {
            if(hit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                // Debug.Log("Clear counter");
                // clearCounter.Interact();
            }
        }
    }

    private void HandleMovement()
    {
        Vector2 input = _gameInput.GetMovementVectorNormalized();     
        Vector3 moveDir = new Vector3(input.x, 0, input.y);
        float moveDistance = moveSpeed * Time.deltaTime;
        isWalking = moveDir != Vector3.zero;
        bool canMove = !Physics.CapsuleCast(transform.position,transform.position+Vector3.up*playerHeight,playerRadius,moveDir,moveDistance);

        if (!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position,transform.position+Vector3.up*playerHeight,playerRadius,moveDirX,moveDistance);
            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove =!Physics.CapsuleCast(transform.position,transform.position+Vector3.up*playerHeight,playerRadius,moveDirZ,moveDistance);
                if (canMove)
                {
                    moveDir = moveDirZ;
                }
            }
            
        }
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    public bool IsWalking() => isWalking;
}

