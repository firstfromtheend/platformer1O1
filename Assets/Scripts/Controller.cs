using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    //test
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float moveSpeed = 10f;

    private PlayerController playerController;
    private Rigidbody2D playerRB;
    private float defaultGravityScale;
    private Animator playerAnimator;

    //colliders
    private BoxCollider2D footBoxCollider;
    private CapsuleCollider2D bodyCapsuleCollider;

    [SerializeField] bool onGround;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        defaultGravityScale = playerRB.gravityScale;
        playerAnimator = GetComponent<Animator>();

        footBoxCollider = GetComponent<BoxCollider2D>();
        bodyCapsuleCollider = GetComponent<CapsuleCollider2D>();

        playerController = new PlayerController();
        playerController.Player.Jump.performed += _ => Jump();
        playerController.Player.Move.performed += _ => Move();
    }

    private void Jump()
    {
        if (onGround)
        {
            playerAnimator.SetBool("PlayerJump", true);
            playerRB.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
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
        float movementInputByY = playerController.Player.Move.ReadValue<Vector2>().y;
        Debug.Log(movementInputByY);
    }

    private void FlipSprite()
    {
        bool playerMoveRightNow = Mathf.Abs(playerRB.velocity.x) > Mathf.Epsilon;
        if (playerMoveRightNow)
        {
            transform.localScale = new Vector2(Mathf.Sign(playerRB.velocity.x), 1f);
        }
    }

    private void Move()
    {
        bool playerMoveRightNow = Mathf.Abs(playerRB.velocity.x) > Mathf.Epsilon;
        playerAnimator.SetBool("PlayerRun", playerMoveRightNow);
        float movementInputByX = playerController.Player.Move.ReadValue<Vector2>().x;
        playerRB.velocity = new Vector2(movementInputByX * moveSpeed, playerRB.velocity.y);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            playerAnimator.SetBool("PlayerRun", false);
            onGround = !onGround;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            onGround = !onGround;
            playerAnimator.SetBool("PlayerJump", false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Climbing")
        {
            bool playerClimbRightNow = Mathf.Abs(playerRB.velocity.y) > Mathf.Epsilon;
            playerAnimator.SetBool("PlayerRun", playerClimbRightNow);
            float movementInputByY = playerController.Player.Move.ReadValue<Vector2>().y;
            Debug.Log(movementInputByY);
            playerRB.velocity = new Vector2(playerRB.velocity.x, movementInputByY * moveSpeed);
            playerRB.gravityScale = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Climbing")
        {
            playerRB.gravityScale = defaultGravityScale;
        }
    }
}
