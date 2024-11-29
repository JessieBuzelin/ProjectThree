using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float crouchSpeed = 2.5f; //Speed while crouching
    public float lookSpeed = 2f;
    public float jumpForce = 5f;
    public float crouchHeight = 0.5f; // Height of the player when crouched
    public float standingHeight = 2f; // Standing height

    private CharacterController characterController;
    private Vector3 moveDirection;
    private float verticalRotation;
    private bool isCrouching = false; // if crouching or not

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        characterController.height = standingHeight; 
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.C)) 
        {
            ToggleCrouch();
        }

        // Movement stuff for character controller
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveHorizontal + transform.forward * moveVertical;

       
        float currentMoveSpeed = isCrouching ? crouchSpeed : moveSpeed;

        characterController.Move(move * currentMoveSpeed * Time.deltaTime);

        // Mouse moves camera for movement
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        Camera camera = Camera.main;
        camera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

       
        if (characterController.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                
                moveDirection.y = jumpForce;
            }
        }

        // Gravity
        moveDirection.y -= 9.81f * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    void ToggleCrouch()
    {
       
        isCrouching = !isCrouching;

        if (isCrouching)
        {
            
            characterController.height = crouchHeight;
        }
        else
        {
           
            characterController.height = standingHeight;
        }
    }
}


