using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{

    public float moveSpeed = 10;
    public bool canMove = true;
    public float defaultPushBackForce;

    private PlayerInput playerInput;
    private InputAction attackAction;
    private InputAction moveAction;
    private Vector2 moveInput;

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
        moveInput = moveAction.ReadValue<Vector2>();

        if (attackAction.triggered)
        {
            print("if (attackAction.triggered)");
        }

        if (canMove)
        {
            ProcessMovement();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 randVec = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
            PushPlayerInRandomDirection(randVec.normalized * defaultPushBackForce);
        }
    }

    private void ProcessMovement()
    {
        rb2D.linearVelocity = (moveInput * Time.deltaTime * moveSpeed);
    }

    private void PushPlayerInRandomDirection(Vector2 force)
    {
        rb2D.AddForce(force, ForceMode2D.Impulse);
        StartCoroutine(DisabledMovementWhilePushed());
    }

    private IEnumerator DisabledMovementWhilePushed()
    {
        canMove = false;
        yield return new WaitForSeconds(1);
        canMove = true;
    }
}