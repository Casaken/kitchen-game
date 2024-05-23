using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private Player _player;

    private const string IS_WALKING = "IsWalking";
    // Start is called before the first frame update
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetBool(IS_WALKING, _player.IsWalking());
    }
}
