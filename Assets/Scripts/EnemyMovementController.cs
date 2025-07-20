using System;
using UnityEngine;
using Pathfinding;

public class EnemyMovementController : MonoBehaviour
{
    public enum MoveStates { idle, chasing, attacking }

    [Header("Setup - Leave empty")]
    public MoveStates currentMoveState = MoveStates.idle;
    public GameObject targetObject;
    [SerializeField]
    private GameObject playerObject;
    [SerializeField]
    private bool targetIsInRange = false;
    [SerializeField]
    private float distFromTarget;
    [SerializeField]
    private bool inContactWithPlayer;
    [SerializeField]
    private Rigidbody2D rb2D;

    [Header("Config - Set enemy stats")]
    [SerializeField]
    private float chaseDist;
    public float moveSpeed;
    public float normalSpeed;

    [SerializeField]
    public float nextWaypointDistance = 3f;

    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false; // May not be needed
    private Seeker seeker;
    
    void Start()
    {
        SetPlayerObject();
        SetTargetasPlayer();

        rb2D = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        normalSpeed = moveSpeed;

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
        // A* causes rotation to become stilted - smoothen out
        if (currentMoveState != MoveStates.chasing) { return; }

        Vector2 _moveDir = ((Vector2)targetObject.transform.position - (Vector2)transform.position).normalized;

        // Do not rotate while attacking - more souls-like
        if (currentMoveState != MoveStates.attacking) { RotatePlayerBody(_moveDir);}
    }

    private void ManageMovement()
    {
        if (targetObject == null) { return; }
        if (path == null) { return; }
        if (currentMoveState == MoveStates.attacking) { return; }

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
        else if (currentMoveState == MoveStates.chasing)
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
        if (dist <= chaseDist && currentMoveState != MoveStates.attacking)
        {
            currentMoveState = MoveStates.chasing;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            inContactWithPlayer = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            inContactWithPlayer = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            inContactWithPlayer = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Attack distance
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDist);
    }
}
