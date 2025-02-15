using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private const string IS_WALKING ="IsWalking";
    [SerializeField] PlayerMovement playerMovement;
    // Start is called before the first frame update
    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool(IS_WALKING, false);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool(IS_WALKING,playerMovement.IsWalking());
    }
}
