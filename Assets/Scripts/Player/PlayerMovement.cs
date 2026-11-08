using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;

    [SerializeField] private float acceleration;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float deceleration;

    [SerializeField] private float jumpForce;

    [SerializeField] private float dashForce;

    [SerializeField] private float gravity;

    [SerializeField] private float turnSpeed;

    private bool _isDashing;

    private Vector3 _velocity;
    private float _verticalVel;

    private Vector3 _cachedDir;

    private float _yRotation = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Jump(InputManager.jumpInput);
        Move(InputManager.movementInput);
        Turn(InputManager.mouseLookInput);
    }

    private void Move(Vector2 moveInput)
    {
        moveInput = moveInput.normalized;

        if (!_isDashing)
        {
            _cachedDir = transform.forward * moveInput.y + transform.right * moveInput.x;
        }

        if(moveInput.magnitude != 0)
        {
            _velocity = Vector3.Lerp(_velocity, _cachedDir * walkSpeed, acceleration/100 * Time.fixedDeltaTime);
        }
        else
        {
            _velocity = Vector3.Lerp(_velocity, Vector3.zero, deceleration * Time.fixedDeltaTime);
        }

        if (!GroundCheck())
        {
            _verticalVel -= gravity * Time.fixedDeltaTime;
        }
        else
        {
            if (Jump(InputManager.jumpInput))
            {
                _verticalVel = jumpForce / 100;
            }
            else
            {
                _verticalVel = 0;
            }
        }

        _velocity.y = _verticalVel;

        characterController.Move(_velocity);

    }

    private bool GroundCheck()
    {
        return characterController.isGrounded;
    }

    private bool Jump(float jumpInput)
    {
        return jumpInput != 0;
    }

    private void Turn(Vector2 turnInput)
    {
        Debug.Log(turnInput);
        _yRotation += turnInput.x * turnSpeed;
        transform.localRotation = Quaternion.Euler(0, _yRotation, 0);
    }
}
