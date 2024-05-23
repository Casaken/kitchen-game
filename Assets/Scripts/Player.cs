using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 10f;
    private bool isWalking;
   
    private void Update()
    {
        Vector2 input = new Vector2(0, 0);
        if(Input.GetKey(KeyCode.W))
        {
            input.y = +1;
        }
        if(Input.GetKey(KeyCode.S))
        {
            input.y = -1;
        }
        
        if(Input.GetKey(KeyCode.A))
        {
            input.x = -1;
        }
        if(Input.GetKey(KeyCode.D))
        {
            input.x = +1;
        }
        
        input = input.normalized;
        Vector3 moveDir = new Vector3(input.x, 0, input.y);
        isWalking = moveDir != Vector3.zero;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        transform.forward =Vector3.Slerp(transform.forward,moveDir,Time.deltaTime *rotateSpeed);
    }

    public bool IsWalking() => isWalking;

}
