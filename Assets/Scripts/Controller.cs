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
    private GameObject childWithSprite;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();

        playerController = new PlayerController();
        playerController.Player.Jump.performed += _ => Jump();
        playerController.Player.Move.performed += ctx => Move();
        childWithSprite = this.gameObject.transform.GetChild(0).gameObject;
    }

    private void Jump()
    {
        playerRB.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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
    }

    private void FlipSprite()
    {
        bool playerMoveRightNow = Mathf.Abs(playerRB.velocity.x) > Mathf.Epsilon;
        if (playerMoveRightNow)
        {
            childWithSprite.transform.localScale = new Vector2(Mathf.Sign(playerRB.velocity.x), 1f);
        }
    }

    private void Move()
    {
        float movementInputByX = playerController.Player.Move.ReadValue<Vector2>().x;
        playerRB.velocity = new Vector2(movementInputByX * moveSpeed, playerRB.velocity.y);
    }
}
