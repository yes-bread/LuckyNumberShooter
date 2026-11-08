using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager inputManager;
    public static PlayerInput playerInput;

    [SerializeField] private InputActionReference movement;
    [SerializeField] private InputActionReference jump;
    [SerializeField] private InputActionReference mouseLook;

    [HideInInspector] public static Vector2 movementInput;
    [HideInInspector] public static Vector2 mouseLookInput;
    [HideInInspector] public static float jumpInput;

    void Awake()
    {
        if(inputManager != null)
        {
            Destroy(this);
        }
        
        inputManager = this;
    }

    // Update is called once per frame
    void Update()
    {
        movementInput = movement.action.ReadValue<Vector2>();
        mouseLookInput = mouseLook.action.ReadValue<Vector2>();
        jumpInput = jump.action.ReadValue<float>();
    }
}
