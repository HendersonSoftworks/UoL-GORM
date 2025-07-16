using System;
using UnityEngine;


public class EnemyMovementController : MonoBehaviour
{
    public enum MoveStates { idle, chasing, attacking }

    [Header("Setup - Leave empty")]
    [SerializeField]
    public MoveStates currentMoveState;
    [SerializeField]
    private GameObject targetObject;
    [SerializeField]
    private GameObject playerObject;
    [SerializeField]
    private bool targetIsInRange = false;
    [SerializeField]
    private float distFromTarget;
    [SerializeField]
    private bool isChasing = false;
    [SerializeField]
    private bool inContactWithPlayer;
    [SerializeField]
    private Rigidbody2D rb2D;

    [Header("Config - Set enemy stats")]
    [SerializeField]
    private float attackDist;
    [SerializeField]
    private float moveSpeed;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        SetPlayerObject();
        SetTargetasPlayer();   
    }

    void Update()
    {
        DetectIfTargetInRange();
        ManageMovement();
    }

    private void ManageMovement()
    {
        if (targetObject == null) { return; }

        if (inContactWithPlayer)
        {
            rb2D.linearVelocity = Vector2.zero;
        }
        else if (isChasing)
        {
            Vector2 moveDir = (targetObject.transform.position - transform.position).normalized;
            rb2D.linearVelocity = (moveDir * moveSpeed * Time.deltaTime);
        }
    }

    private void DetectIfTargetInRange()
    {
        if (targetObject == null) { return; }

        float dist = Vector2.Distance(transform.position,
            targetObject.transform.position);
        if (dist <= attackDist)
        {
            isChasing = true;
        }
    }

    private void SetPlayerObject()
    {
        playerObject = FindAnyObjectByType<PlayerMovementController>().gameObject;
    }

    private void SetTargetasPlayer()
    {
        if (playerObject == null) { return; }

        targetObject = playerObject;
    }

    private void OnDrawGizmos()
    {
        // Attack distance
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDist);
    }
}
