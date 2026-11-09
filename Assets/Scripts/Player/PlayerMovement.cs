using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;

    [SerializeField] private float acceleration;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float friction;

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
        ScalePhysicsValues();
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

        /*if(moveInput.magnitude != 0)
        {
            _velocity = Vector3.Lerp(_velocity, _cachedDir * walkSpeed, acceleration/100 * Time.fixedDeltaTime);
        }
        else
        {
            _velocity = Vector3.Lerp(_velocity, Vector3.zero, deceleration * Time.fixedDeltaTime);
        }*/
        _velocity = ApplyFriction(_velocity, friction);

        _velocity = Accelerate(_velocity, _cachedDir, acceleration, walkSpeed);

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
        //Debug.Log(turnInput);
        _yRotation += turnInput.x * turnSpeed;
        transform.localRotation = Quaternion.Euler(0, _yRotation, 0);
    }

    private Vector3 Accelerate(Vector3 velocity, Vector3 direction, float acceleration, float fullSpeed){
        float difference = Vector3.Dot(velocity, direction);

        Debug.Log(difference);

        float speedInDir = fullSpeed - difference;

        if(speedInDir <= 0f){
            return velocity;
        }

        float accelerationInDir = Mathf.Min(speedInDir, acceleration);

        return velocity + direction * accelerationInDir * Time.fixedDeltaTime;
    }

    private Vector3 ApplyFriction(Vector3 velocity, float friction){
        float currentAcceleration = velocity.magnitude;

        if(currentAcceleration <= 0.00001f){
            return Vector3.zero;
        }

        float speedLoss = currentAcceleration * friction * Time.fixedDeltaTime;
        float newSpeed = Mathf.Max(currentAcceleration - speedLoss, 0f); 

        return velocity * (newSpeed / currentAcceleration);
    }

    private void ScalePhysicsValues(){
        walkSpeed /= 10;
        //acceleration /= 10;
    }
}
