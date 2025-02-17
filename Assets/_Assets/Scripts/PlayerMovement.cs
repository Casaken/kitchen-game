using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float playerSize = 0.7f;
    [SerializeField] private LayerMask counterLayerMask;
    private float playerHeight = 2f;
    private bool _canMove;
    private Vector3 lastInteractDirection;

    private bool isWalking;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleInteractions();
       
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleMovement()
    {
        float moveDistance= moveSpeed * Time.deltaTime;
        Vector2 inputVector = playerInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        _canMove = !Physics.CapsuleCast(transform.position,transform.position+Vector3.up*playerHeight, playerSize, moveDir, moveDistance);
        if (!_canMove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            _canMove = !Physics.CapsuleCast(transform.position,transform.position+Vector3.up*playerHeight, playerSize, moveDirX, moveDistance);
            if (_canMove)
            {
                moveDir =moveDirX;;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                _canMove = !Physics.CapsuleCast(transform.position,transform.position+Vector3.up*playerHeight, playerSize, moveDirZ, moveDistance);
                if (_canMove)
                {
                    moveDir = moveDirZ;
                }
            }
        }
        if (_canMove)
        {
            transform.position += moveDir * moveDistance;
        }
        if (moveDir != Vector3.zero)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
        transform.forward =Vector3.Slerp(transform.forward,moveDir, Time.deltaTime * rotationSpeed);
        Debug.Log(inputVector);
    }

    private void HandleInteractions()
    {
        float interactDistance = 2f;
        Vector3 inputVector = playerInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        
        if (moveDir != Vector3.zero)
        {
            lastInteractDirection = moveDir;
        }

        if (Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit hit, interactDistance, counterLayerMask))
        {
            if (hit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                clearCounter.Interact();
            }
        }
    }
}
