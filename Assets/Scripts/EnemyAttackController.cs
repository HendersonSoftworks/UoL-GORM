using UnityEngine;
using System.Collections;

public class EnemyAttackController : MonoBehaviour
{
    [Header("Setup - Leave empty")]
    [SerializeField]
    private EnemyMovementController movementController;

    [Header("Config - Customise Enemy")]
    [SerializeField]
    private float attackWarmupTimer = 0;
    [SerializeField]
    private float attackCooldownTimer = 0;
    [SerializeField]
    private float attackDist = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementController = GetComponent<EnemyMovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        ManageAttacking();
    }

    private void ManageAttacking()
    {
        if (movementController.targetObject == null) { return; }

        var attackDist = Vector2.Distance(movementController.targetObject.transform.position, transform.position);
        if (attackDist <= 0)
        {
            StartAttackWarmupTimer();
        }
    }

    private IEnumerator StartAttackWarmupTimer()
    {
        yield return new WaitForSeconds(attackWarmupTimer);
        AttackTarget(movementController.targetObject);
        StartAttackCooldownTimer();
    }

    private IEnumerator StartAttackCooldownTimer()
    {
        yield return new WaitForSeconds(attackCooldownTimer);
    }

    private void AttackTarget(GameObject target)
    {
        movementController.currentMoveState = EnemyMovementController.MoveStates.attacking;

        // Attack stuff
    }

    

    private void OnDrawGizmosSelected()
    {
        // Attack distance
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDist);
    }

}
