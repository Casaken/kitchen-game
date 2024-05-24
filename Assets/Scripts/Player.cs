using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    private float playerRadius = 0.7f;
    private float playerHeight = 2f;
    private bool isWalking;
    private ClearCounter selectedCounter;
    private Vector3 lastInteractDirection;

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged ;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public ClearCounter selectedCounter;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
    }

    private void Start()
    {
        _gameInput.OnInteract += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact();
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
                //this clear counter is the output of raycast. I did not define it at the start. Thats how it calls the clearcounter function btw :().
                //if the counter we are looking at is not the selected one then we select it for sure.
                if (clearCounter != selectedCounter)
                {
                    SetSelectedCounter(clearCounter);

                }
            }
            else
            {
                //if it hits something that is not a clear counter then we set the selected one to null.
                SetSelectedCounter(null); 
                
            }
        }else
        {
            SetSelectedCounter(null);
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

    private void SetSelectedCounter(ClearCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }
}

