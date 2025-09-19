using System;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Boss Logic")]
    [SerializeField]
    private bool isAttacking = false;
    public Spell[] spells;
    public GameObject[] telePoints;

    [Header("References - Loaded on start")]
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<PlayerMovementController>().gameObject;
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Debugging
        if (isAttacking)
        {
            SetAttackState(true);
        }

        ManageRotation();
    }

    public void TeleportToRandomPoint()
    {
        var randPoint = UnityEngine.Random.Range(0, telePoints.Length);

        // Explosion effect

        transform.position = telePoints[randPoint].transform.position;
    }

    public void SetAttackState(bool value)
    {
        isAttacking = value;
        animator.SetBool("isAttacking", value);
    }

    public void FireRandSpell(int randInt)
    {
        var enemy = gameObject.GetComponent<EnemyCharacter>();
        spells[randInt].Cast(enemy, transform.up);
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

    private void ManageRotation()
    {
        if (player == null) { return;}

        Vector2 _moveDir = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;

        RotatePlayerBody(_moveDir); 
    }

}
