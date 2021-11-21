using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour, IMove
{
    [Range(0f,2f)] public float moveAmount;
    [Range(0f, 100f)] public float jumpAmount;
    [Range(0f, 0.5f)] public float sprintAmount;

    public CollisionHandler bottomCollisionHandler;
    public bool onGround;
    public Rigidbody rigidBody;

    private Vector3 moveVect;
    private bool shouldMove;
    private float defaultMoveAmount;

    void Start()
    {
        moveVect = new Vector3(0, 0, 0);
        shouldMove = false;
        defaultMoveAmount = moveAmount;
    }

    void Update()
    {
        onGround = bottomCollisionHandler.isOnGround;

        HandleSprint();

        HandleMove();
        
        HandleJump();

        if (shouldMove)
        { 
            // apply the value to game object's transform
            transform.position += moveVect;

            // reset control parameters
            shouldMove = false;
            moveVect *= 0;
        }
    }

    private void HandleSprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            moveAmount += sprintAmount;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            moveAmount = defaultMoveAmount;
        }
    }

    public void HandleMove()
    {
        if (Input.GetKey(KeyCode.W))
        {
            moveVect.z += moveAmount;
            shouldMove = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveVect.z -= moveAmount;
            shouldMove = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveVect.x += moveAmount;
            shouldMove = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveVect.x -= moveAmount;
            shouldMove = true;
        }
    }

    public void HandleJump()
    {
        if (!onGround)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigidBody.AddForce(transform.up * jumpAmount, ForceMode.Impulse);
        }
    }
}
