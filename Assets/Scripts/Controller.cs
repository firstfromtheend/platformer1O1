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
    private CapsuleCollider2D playerBodyCollider;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        defaultGravityScale = playerRB.gravityScale;
        playerAnimator = GetComponent<Animator>();

        footBoxCollider = GetComponent<BoxCollider2D>();
        playerBodyCollider = GetComponent<CapsuleCollider2D>();

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
        TriggerPlayerdeath();
        Move();
        FlipSprite();
        ClimbingOnLadders();
    }

    private void TriggerPlayerdeath()
    {
        if (playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")) || playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")))
        {
            playerController.Player.Disable();
            playerAnimator.SetTrigger("Die");
            //OnDisable();
            //Debug.Log("gg wp");
        }
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


    //just in case another way to desable player movement, can freely delete method below
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Enemy")
    //    {
    //        OnDisable();
    //    }
    //}
}
