using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    private float spd = 15;
    [SerializeField]
    private float lookSensitivity = 25;
    [SerializeField]
    private float jumpHeight = 1;
    [SerializeField]
    private float gravity = -9.81f;

    private Vector2 moveVector;
    private Vector2 lookVector;
    private Vector3 rotation;
    private float verticalVelocity;
    private Animator animator;

    private CharacterController characterController;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotation();
    }

    public void OnMove(InputAction.CallbackContext context) {
        moveVector = context.ReadValue<Vector2>();

        animator.SetBool("isSprinting", false);
        animator.SetBool("isSprintingHardLeft", false);
        animator.SetBool("isSprintingLeft", false);
        animator.SetBool("isSprintingHardRight", false);
        animator.SetBool("isSprintingRight", false);

        if (moveVector.magnitude > 0) {
            if (moveVector.x < 0 && moveVector.y == 0) {
                // Only left (A key pressed)
                animator.SetBool("isSprintingHardLeft", true);
            }
            else if (moveVector.x < 0 && moveVector.y > 0) {
                // Forward left (W and A pressed)
                animator.SetBool("isSprintingLeft", true);
            }
            else if (moveVector.x > 0 && moveVector.y == 0) {
                // Only right (D key pressed)
                animator.SetBool("isSprintingHardRight", true);
            }
            else if (moveVector.x > 0 && moveVector.y > 0) {
                // Forward right (W and D pressed)
                animator.SetBool("isSprintingRight", true);
            }
            else if (moveVector.x == 0 && moveVector.y > 0) {
                // Only forward (W key pressed)
                animator.SetBool("isSprinting", true);
            }
        }
        else {
            animator.SetBool("isSprinting", false);
        }
    }

    private void Move() {
        verticalVelocity += gravity * Time.deltaTime;

        if(characterController.isGrounded && verticalVelocity < 0) {
            verticalVelocity = gravity*Time.deltaTime;

        }

        Vector3 move = transform.right*moveVector.x + transform.forward*moveVector.y + transform.up*verticalVelocity;
        characterController.Move(move*spd*Time.deltaTime);
    }

    public void OnLook(InputAction.CallbackContext context) {
        lookVector = context.ReadValue<Vector2>();
    }

    private void Rotation() {
        rotation.y += lookVector.x * lookSensitivity * Time.deltaTime;
        transform.localEulerAngles = rotation;
    }

    public void onJump(InputAction.CallbackContext context) {
        if(characterController.isGrounded && context.performed) {
            animator.Play("BasicMotions@Jump01 [RM]");
            Jump();
        }
    }

    private void Jump() {
        verticalVelocity = Mathf.Sqrt(jumpHeight*-gravity);
    }
}
