using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovementAndAnimationController : MonoBehaviour
{
    //reference variables
    PlayerInput playerInput;
    CharacterController characterController;
    Animator animator;

    //variables to store player input
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    bool isMovementPressed;
    bool isJumpPressed = false;
    bool isSpinPressed = false;

    //value types
    [SerializeField] float rotationFactorPerFrame = 15.0f;
    [SerializeField] float speed = 2.0f;

    //gravity
    float gravity = -9.8f;
    float groundedGravity = -2.0f;
    [SerializeField] float fallMultiplier = 2.0f;

    //JUmping Varaibles
    [SerializeField] float initialJumpVelocity;
    [SerializeField] float maxJumpHeight = 1.0f;
    [SerializeField] float maxJumpTime = 0.5f;
    bool isJumping = false;
    bool isJumpingAnimating = false;
    bool isSpining = false;
    void Awake()
    {
        //initializing refernece variables
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        playerInput = new PlayerInput();
        //player input callback function
        //performed for diagonal movement
        playerInput.CharacterControls.Move.performed += ReadMovementInput; 
        playerInput.CharacterControls.Move.canceled += ReadMovementInput;
        playerInput.CharacterControls.Jump.started += OnJump;
        playerInput.CharacterControls.Jump.canceled += OnJump;
        playerInput.CharacterControls.Spin.started += OnSpin;
        playerInput.CharacterControls.Spin.canceled += OnSpin;

        InitializeJumpParameters();
    }
    void OnSpin(InputAction.CallbackContext context)
    {
        isSpinPressed = context.ReadValueAsButton();
    }
    void OnJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }
    void ReadMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        //Debug.Log(currentMovementInput);
        currentMovement.x = currentMovementInput.x * speed;
        currentMovement.z = currentMovementInput.y * speed;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }
    void Update()
    {
        MovePlayer();
        HandleJump();
        HandleSpin();
    }

    private void HandleSpin()
    {
        if (isSpinPressed && !isSpining)
        {
            animator.SetTrigger("Spin");
            isSpining = true;
        }
        else if (!isSpinPressed && isSpining)
        {
            isSpining = false;
        }
    }
    public bool GetIsSpining()
    {
        return isSpining;
    }
    private void InitializeJumpParameters()
    {
        float timeToApex = maxJumpHeight / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }
    private void HandleJump()
    {
        if(!isJumping && characterController.isGrounded && isJumpPressed)
        {
            isJumping = true;
            animator.SetBool("IsJumping", true);
            isJumpingAnimating = true;
            currentMovement.y = initialJumpVelocity;
        }
        else if (isJumping && !isJumpPressed && characterController.isGrounded)
        {
            isJumping = false;
        }
    }
    private void MovePlayer()
    {
        HandleAniamtion();
        HandleRotation();
        characterController.Move(currentMovement * Time.deltaTime);
        HandleGravity();
    }

    private void HandleGravity()
    {
        bool isFalling = currentMovement.y <= 0.0d || !isJumpPressed;
        if (characterController.isGrounded)
        {
            currentMovement.y = groundedGravity;
            if (isJumpingAnimating)
            {
                animator.SetBool("IsJumping", false);
            }
        }
        else if (isFalling) //increasing velocity when player is fallling
        {
            float previousYVelocity = currentMovement.y;
            float newYVelocity = previousYVelocity + (gravity * fallMultiplier * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
            currentMovement.y = nextYVelocity; 
        }
        else //use Velocity verlet integration instead of euler integeration
        {
            float previousYVelocity = currentMovement.y;
            float newYVelocity = previousYVelocity + (gravity * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
            currentMovement.y = nextYVelocity; 
        }
    }
    private void HandleRotation()
    {
        Vector3 positionToLookAt;
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;

        if (isMovementPressed)
        {
            Quaternion currentRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
    }
    private void HandleAniamtion()
    {
        if (isMovementPressed)
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
    }

    void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }
    void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }
}
