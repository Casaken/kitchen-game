using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }
    public event EventHandler< OnSelectedCounterChangedEventArgs > OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public ClearCounter selectedCounter;
    }
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float playerSize = 0.7f;
    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] float interactDistance = 2f;
    private ClearCounter selectedCounter;
    private float playerHeight = 2f;
    private bool _canMove;
    private Vector3 lastInteractDirection;

    private bool isWalking;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one player instance");
        }
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerInput.OnInteractAction += PlayerInputOnOnInteractAction;  
    }

    private void PlayerInputOnOnInteractAction(object sender, EventArgs e)
    {
        if (selectedCounter !=null)
        {
            selectedCounter.Interact();
        }
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
    }

    private void HandleInteractions()
    {
       
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
                if (clearCounter != selectedCounter)
                {
                    SetSelectedCounter(clearCounter);
                }
            }else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
        
        
    }

    private void SetSelectedCounter(ClearCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }
}
