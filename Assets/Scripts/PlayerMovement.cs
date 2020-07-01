using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Walking,
    Attacking
}

public class PlayerMovement : MonoBehaviour
{
    
    public float moveSpeed = 5f;
    public Rigidbody2D rBody;
    public Animator animator;

    private PlayerState _currentState;
    private Vector2 _movement;
    
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int DoAttack = Animator.StringToHash("DoAttack");

    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        _currentState = PlayerState.Walking;
    }

    // Update is called once per frame
    void Update()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");

        if (_movement != Vector2.zero)
        {
            animator.SetFloat(Horizontal, _movement.x);
            animator.SetFloat(Vertical, _movement.y);
        }
        
        // ReSharper disable once Unity.UnknownInputAxes
        if (Input.GetButtonDown("Attack") && _currentState != PlayerState.Attacking)
        {
            StartCoroutine(Attacking());
        }
        
        animator.SetFloat(Speed, _movement.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        if (_currentState == PlayerState.Walking)
        {
            rBody.MovePosition(rBody.position + _movement * (moveSpeed * Time.fixedDeltaTime));   
        }
    }

    private IEnumerator Attacking()
    {
        animator.SetBool(DoAttack, true);
        _currentState = PlayerState.Attacking;
        yield return null;
        
        animator.SetBool(DoAttack, false);
        yield return new WaitForSeconds(0.3f);
        _currentState = PlayerState.Walking;
    }
}
