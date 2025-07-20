using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    [Header("Setup - Loaded on start")]
    [SerializeField]
    private EnemyMovementController movementController;
    [SerializeField]
    private Animator animator;

    void Start()
    {
        movementController = GetComponent<EnemyMovementController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        ManageAnimationStates();
    }

    public void SetState(EnemyMovementController.MoveStates moveState)
    {
        switch (moveState)
        {
            case EnemyMovementController.MoveStates.idle:
                animator.SetBool("isChasing", false);
                animator.SetBool("isAttacking", false);

                break;
            case EnemyMovementController.MoveStates.chasing:
                animator.SetBool("isChasing", true);
                animator.SetBool("isAttacking", false);

                break;
            case EnemyMovementController.MoveStates.attacking:
                animator.SetBool("isChasing", false);
                animator.SetBool("isAttacking", true);

                break;
            default:
                break;
        }
    }

    private void ManageAnimationStates()
    {
        switch (movementController.currentMoveState)
        {
            case EnemyMovementController.MoveStates.idle:
                animator.SetBool("isChasing", false);
                animator.SetBool("isAttacking", false);
                
                break;
            case EnemyMovementController.MoveStates.chasing:
                animator.SetBool("isChasing", true);
                animator.SetBool("isAttacking", false);
                
                break;
            case EnemyMovementController.MoveStates.attacking:
                animator.SetBool("isChasing", false);
                animator.SetBool("isAttacking", true);

                break;
            default:
                break;
        }
    }
}
