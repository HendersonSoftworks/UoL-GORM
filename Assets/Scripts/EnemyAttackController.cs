using UnityEngine;
using System.Collections;

public class EnemyAttackController : MonoBehaviour
{
    public enum AttackStates { ready, warmup, attacking, cooldown };

    [Header("Setup - Loaded on start")]
    [SerializeField]
    private EnemyMovementController movementController;

    [Header("Config - Enemy")]
    public AttackStates attackState = AttackStates.ready;
    [SerializeField]
    private float attackWarmupTimer;
    private float attackWarmupTimerReset;
    [SerializeField]
    private float attackCooldownTimer;
    private float attackCooldownTimerReset;
    [SerializeField]
    private float attackDist = 0;

    #region System Methods

    void Start()
    {
        movementController = GetComponent<EnemyMovementController>();

        attackWarmupTimerReset = attackWarmupTimer;
        attackCooldownTimerReset = attackCooldownTimer;
    }

    void Update()
    {
        ManageAttackState();
    }

    #endregion

    #region Public Methods

    #endregion

    #region Private Methods

    private void ManageAttackState()
    {
        switch (attackState)
        {
            case AttackStates.ready:
                ManageReadyToWarmup();
                break;

            case AttackStates.warmup:
                ManageWarmupToAttacking();

                break;
            case AttackStates.attacking:
                ManageAttackingingToCooldown();

                break;
            case AttackStates.cooldown:
                ManageCooldownToReady();

                break;
            default:
                break;
        }
    }

    private void ManageReadyToWarmup()
    {
        // Switch to warming up if ready to attack and close to player
        if (movementController.targetObject == null) { return; }

        var distFromTarget = Vector2.Distance(movementController.targetObject.transform.position, transform.position);
        if (distFromTarget <= attackDist && attackState == AttackStates.ready)
        {
            attackState = AttackStates.warmup;
        }
    }

    private void ManageWarmupToAttacking()
    {
        attackWarmupTimer -= Time.deltaTime;
        if (attackWarmupTimer <= 0)
        {
            attackWarmupTimer = attackWarmupTimerReset;
            attackState = AttackStates.attacking;
        }
    }

    private void ManageAttackingingToCooldown()
    {
        // Change move state to attacking as well
        movementController.currentMoveState = EnemyMovementController.MoveStates.attacking;

        // Wait for attack anim to give signal
        print("waiting for attack animation finish...");

    }

    private void ManageCooldownToReady()
    {
        attackCooldownTimer -= Time.deltaTime;
        if (attackCooldownTimer <= 0)
        {
            attackCooldownTimer = attackCooldownTimerReset;
            attackState = AttackStates.ready;
        }
    }


    private void OnDrawGizmosSelected()
    {
        // Attack distance
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDist);
    }

    #endregion
}
