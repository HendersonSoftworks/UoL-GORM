using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    public float moveSpeed = 10;

    private PlayerInput playerInput;
    private InputAction attackAction;
    private InputAction moveAction;

    private Rigidbody2D rb2D;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}


