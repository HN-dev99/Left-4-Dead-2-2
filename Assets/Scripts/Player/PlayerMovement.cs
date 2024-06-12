using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    [SerializeField] private float playerSpeed = 10f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float gravity = -9.81f * 2;
    [SerializeField] private bool groundedPlayer;

    public float initialHeight;
    public float sitHeight = 0.5f;
    Vector3 velocity;

    bool isMoving;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        initialHeight = controller.height;
    }


    private void Update()
    {
        groundedPlayer = controller.isGrounded;

        if (groundedPlayer && velocity.y < 0)
        {
            velocity.y = -5f;
        }

        SitDown();
        StandUp();

        // Getting Input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //Move

        Vector3 move = transform.right * x + transform.forward * z;
        move.y = 0;
        controller.Move(move * playerSpeed * Time.deltaTime);


        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            velocity.y = Mathf.Sqrt(-2f * gravity * jumpHeight);
        }
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);


    }


    private void SitDown()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            controller.height = sitHeight;
        }
    }

    private void StandUp()
    {
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            controller.height = initialHeight;
        }
    }
}
