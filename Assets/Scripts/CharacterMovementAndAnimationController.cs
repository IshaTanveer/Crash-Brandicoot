using System;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovementAndAnimationController : MonoBehaviour
{
    PlayerInput playerInput;
    CharacterController characterController;
    Animator animator;
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    bool isMovementPressed;
    [SerializeField] float rotationFactorPerFrame = 15.0f;
    [SerializeField] float speed = 2.0f;
    void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        playerInput = new PlayerInput();
        playerInput.CharacterControls.Move.started += ReadMovementInput;
        playerInput.CharacterControls.Move.canceled += ReadMovementInput;
    }
    void ReadMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x * speed;
        currentMovement.z = currentMovementInput.y * speed;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }
    void Update()
    {
        HandleAniamtion();
        HandleRotation();
        characterController.Move(currentMovement * Time.deltaTime);
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
