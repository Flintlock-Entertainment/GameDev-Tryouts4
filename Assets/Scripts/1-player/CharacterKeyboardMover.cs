using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


/**
 * This component moves a player controlled with a CharacterController using the keyboard.
 */
[RequireComponent(typeof(CharacterController))]
public class CharacterKeyboardMover: MonoBehaviour {
    [Tooltip("Speed of player keyboard-movement, in meters/second")]
    [SerializeField] float speed = 3.5f;
    [SerializeField] float sprintSpeed = 1.5f;
    [SerializeField] float gravity = 9.81f;

    private CharacterController cc;

    [SerializeField] InputAction moveAction;

    [SerializeField] InputAction SprintAction;
    private void OnEnable() { 
        moveAction.Enable();
        SprintAction.Enable();
    }
    private void OnDisable() { 
        moveAction.Disable();
        SprintAction.Disable();
    }
    void OnValidate() {
        // Provide default bindings for the input actions.
        // Based on answer by DMGregory: https://gamedev.stackexchange.com/a/205345/18261
        if (moveAction == null)
            moveAction = new InputAction(type: InputActionType.Button);
        if (moveAction.bindings.Count == 0)
            moveAction.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/W")
                .With("Down", "<Keyboard>/S")
                .With("Left", "<Keyboard>/A")
                .With("Right", "<Keyboard>/D");
        if (SprintAction == null)
            SprintAction = new InputAction(type: InputActionType.Button);
        if (SprintAction.bindings.Count == 0)
            SprintAction.AddBinding("Sprint", "<Keyboard>/LeftShift");
    }

    void Start() {
        cc = GetComponent<CharacterController>();
    }

    Vector3 velocity = new Vector3(0,0,0);

    void Update()  {
        if (cc.isGrounded) {
            Vector3 movement = moveAction.ReadValue<Vector2>(); // Implicitly convert Vector2 to Vector3, setting z=0.
            var sprint = SprintAction.ReadValue<float>();
            velocity.x = movement.x * (speed + sprint*sprintSpeed);
            velocity.z = movement.y * (speed + sprint * sprintSpeed);
        } else {
            velocity.y -= gravity*Time.deltaTime;
        }

        // Move in the direction you look:
        velocity = transform.TransformDirection(velocity);

        cc.Move(velocity * Time.deltaTime);
    }
}
