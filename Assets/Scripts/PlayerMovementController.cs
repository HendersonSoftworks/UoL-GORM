using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Loaded on start")]
    public float moveSpeed = 10;
    public float baseSpeed = 10;
    public float attackMoveSpeed;
    public bool canMove = true;
    public float pushTime;
    public Vector2 moveInput;
    public PlayerInput playerInput;
    public InputAction attackAction;
    public InputAction moveAction;
    [SerializeField]
    private Rigidbody2D rb2D;
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private PlayerCharacter playerCharacter;
    [SerializeField]
    private PlayerAttackController attackController;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        playerCharacter = GetComponent<PlayerCharacter>();
        attackController = GetComponent<PlayerAttackController>();

        rb2D = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];

        baseSpeed = moveSpeed;
    }

    void FixedUpdate()
    {
        if (gameManager.isGamePaused) { return; }
        if (attackController.isCasting) { return; }

        moveInput = moveAction.ReadValue<Vector2>();

        if (canMove)
        {
            ProcessMovement();
        }
    }

    #region Public methods
    
    public void PushPlayerInDirection(GameObject player, GameObject enemy, float force = 10, bool randomDir = false)
    {
        Vector2 forceAngle = (player.transform.position - enemy.transform.position).normalized;
        if (randomDir) { forceAngle = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)); }

        rb2D.AddForce(forceAngle * force, ForceMode2D.Impulse);
        StartCoroutine(DisabledMovementWhilePushed());
    }

    public void SetVelToZero()
    {
        rb2D.linearVelocity = Vector2.zero;
    }

    #endregion

    #region Private methods

    private void ProcessMovement()
    {
        rb2D.linearVelocity = (moveInput * Time.deltaTime * moveSpeed);
        RotatePlayerBody(moveInput);
    }

    private void RotatePlayerBody(Vector2 moveInput)
    {
        if (moveInput.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
            angle -= 90f;

            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = targetRotation;
        }
    }

    private IEnumerator DisabledMovementWhilePushed()
    {
        canMove = false;
        yield return new WaitForSeconds(pushTime);
        canMove = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Wall or obstacle
        if (collision.gameObject.layer == 6 ||
            collision.gameObject.layer == 7 &&
            canMove == false)
        {
            canMove = true;
        }
    }

    #endregion 
}

