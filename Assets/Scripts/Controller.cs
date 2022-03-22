using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float moveSpeed = 10f;

    private PlayerController playerController;
    private Rigidbody2D playerRB;
    private float defaultGravityScale;
    private Animator playerAnimator;

    //colliders
    private BoxCollider2D footBoxCollider;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        defaultGravityScale = playerRB.gravityScale;
        playerAnimator = GetComponent<Animator>();

        footBoxCollider = GetComponent<BoxCollider2D>();

        playerController = new PlayerController();
        playerController.Player.Jump.performed += _ => Jump();
        playerController.Player.Move.performed += _ => Move();
    }

    private void OnEnable()
    {
        playerController.Player.Enable();
    }

    private void OnDisable()
    {
        playerController.Player.Disable();
    }

    private void FixedUpdate()
    {
        Move();
        FlipSprite();
        ClimbingOnLadders();
    }

    private void FlipSprite()
    {
        bool playerMoveRightNow = Mathf.Abs(playerRB.velocity.x) > Mathf.Epsilon;
        if (playerMoveRightNow)
        {
            transform.localScale = new Vector2(Mathf.Sign(playerRB.velocity.x), 1f);
        }
    }

    private void Jump()
    {
        if (footBoxCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            playerAnimator.SetBool("PlayerJump", !playerAnimator.GetBool("PlayerJump"));
            playerRB.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void Move()
    {
        if (Mathf.Abs(playerRB.velocity.x) > Mathf.Epsilon)
        {
            playerAnimator.SetBool("PlayerRun", true);
        }
        else
        {
            playerAnimator.SetBool("PlayerRun", false);
        }
        float movementInputByX = playerController.Player.Move.ReadValue<Vector2>().x;
        playerRB.velocity = new Vector2(movementInputByX * moveSpeed, playerRB.velocity.y);
    }

    private void ClimbingOnLadders()
    {
        if (footBoxCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            if (Mathf.Abs(playerRB.velocity.y) > Mathf.Epsilon)
            {
                playerAnimator.SetBool("PlayerClimb", true);
            }
            else
            {
                playerAnimator.SetBool("PlayerClimb", false);
            }
            float movementInputByY = playerController.Player.Move.ReadValue<Vector2>().y;
            playerRB.velocity = new Vector2(playerRB.velocity.x, movementInputByY * moveSpeed);
            playerRB.gravityScale = 0;
        }
        else
        {
            playerRB.gravityScale = defaultGravityScale;
        }
    }
}
