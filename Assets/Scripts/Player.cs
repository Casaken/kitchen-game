using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
   
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
        transform.position += moveDir * Time.deltaTime;
        Debug.Log(input);
    }
}
