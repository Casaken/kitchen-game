using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteract;
    private PlayerInputActions _playerInputActions;
    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Interact.performed += Interact_performed;
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        OnInteract?.Invoke(this,EventArgs.Empty);
    }


    public Vector3 GetMovementVectorNormalized()
    {
        Vector2 input = _playerInputActions.Player.Move.ReadValue<Vector2>();
        
        input = input.normalized;
        return input;
    }
}
