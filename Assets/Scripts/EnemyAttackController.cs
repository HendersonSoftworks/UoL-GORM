using UnityEngine;
using System.Collections;

public class EnemyAttackController : MonoBehaviour
{
    private enum AttackStates { ready, warmup, cooldown };

    [Header("Setup - Leave empty")]
    [SerializeField]
    private EnemyMovementController movementController;

    [Header("Config - Customise Enemy")]
    [SerializeField]
    private AttackStates attackState = AttackStates.ready;
    [SerializeField]
    private float attackWarmupTimer;
    private float attackWarmupTimerReset;
    [SerializeField]
    private float attackCooldownTimer;
    private float attackCooldownTimerReset;

    [SerializeField]
    private float attackDist = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementController = GetComponent<EnemyMovementController>();

        attackWarmupTimerReset = attackWarmupTimer;
        attackCooldownTimerReset = attackCooldownTimer;
    }

    // Update is called once per frame
    void Update()
    {
        switch (attackState)
        {
            case AttackStates.ready:
                ManageReadyToWarmup();
                break;

            case AttackStates.warmup:
                ManageWarmupToAttack();

                break;
            case AttackStates.cooldown:
                ManageAttackToCooldown();

                break;
            default:
                break;
        }
    }

    private void ManageAttackToCooldown()
    {
        throw new System.NotImplementedException();
    }

    private void ManageWarmupToAttack()
    {
        attackWarmupTimer -= Time.deltaTime;
        if (attackWarmupTimer <= 0)
        {
            AttackTarget(movementController.targetObject);
            attackWarmupTimer = attackWarmupTimerReset;
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

    private void StartAttackCooldownTimer()
    {
        attackCooldownTimer -= Time.deltaTime;
        if (attackCooldownTimer <= 0)
        {
            AttackTarget(movementController.targetObject);
            attackCooldownTimer = attackCooldownTimerReset;
        }
    }

    private void AttackTarget(GameObject target)
    {
        movementController.currentMoveState = EnemyMovementController.MoveStates.attacking;

        // Attack stuff

        StartAttackCooldownTimer();
    }

    private void OnDrawGizmosSelected()
    {
        // Attack distance
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDist);
    }
}
