using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovementController : MonoBehaviour
{
    public float moveSpeed = 10;

    private PlayerInput playerInput;
    private InputAction attackAction;
    private InputAction moveAction;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        attackAction = playerInput.actions["Attack"];
        moveAction = playerInput.actions["Move"];
    }

    void Update()
    {
        if (attackAction.triggered)
        {
            print("if (attackAction.triggered)");
        }

        transform.Translate(moveAction.ReadValue<Vector2>() * Time.deltaTime * moveSpeed);
        
    }
}
