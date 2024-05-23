using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private GameInput _gameInput;
    private bool isWalking;
   
    private void Update()
    {
        Vector2 input = _gameInput.GetMovementVectorNormalized();     
        Vector3 moveDir = new Vector3(input.x, 0, input.y);
        isWalking = moveDir != Vector3.zero;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        transform.forward =Vector3.Slerp(transform.forward,moveDir,Time.deltaTime *rotateSpeed);
    }

    public bool IsWalking() => isWalking;

}
