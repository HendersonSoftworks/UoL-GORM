using System;
using UnityEngine;
using Pathfinding;

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
    [SerializeField]
    public float nextWaypointDistance = 3f;

    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private Seeker seeker;
    
    void Start()
    {
        SetPlayerObject();
        SetTargetasPlayer();

        rb2D = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    private void UpdatePath()
    {
        if (!seeker.IsDone()) { return; }

        seeker.StartPath(rb2D.position, targetObject.transform.position, OnPathComplete);
    }

    private void OnPathComplete(Path _path)
    {
        if (!_path.error)
        {
            path = _path;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        DetectIfTargetInRange();
        ManageMovement();
    }

    private void Update()
    {
        ManageRotation();
    }

    private void ManageRotation()
    {
        // A* causes rotation to become stilted - this smoothens it out
        if (!isChasing)
        {
            return;
        }

        Vector2 _moveDir = ((Vector2)targetObject.transform.position - (Vector2)transform.position).normalized;

        RotatePlayerBody(_moveDir);
    }

    private void ManageMovement()
    {
        if (targetObject == null) { return; }

        if (path == null) { return; }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        if (inContactWithPlayer)
        {
            rb2D.linearVelocity = Vector2.zero;
        }
        else if (isChasing)
        {
            Vector2 moveDir = ((Vector2)path.vectorPath[currentWaypoint] - rb2D.position).normalized;
            Vector2 force = moveDir * moveSpeed * Time.deltaTime;

            float distance = Vector2.Distance(rb2D.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance) { currentWaypoint++; }

            rb2D.linearVelocity = force;
        }
    }

    private void RotatePlayerBody(Vector2 _moveDir)
    {

        // Bit of duplication here - move to helper class
        if (_moveDir.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(_moveDir.y, _moveDir.x) * Mathf.Rad2Deg;
            angle -= 90f;

            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = targetRotation;
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
