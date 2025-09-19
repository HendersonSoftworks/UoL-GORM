using System;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField]
    private EnemyCharacter enemyCharacter;

    public Slider healthSlider;
    public GameObject canvas;

    private PlayerCharacter playerCharacter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<PlayerMovementController>().gameObject;
        playerCharacter = player.GetComponent<PlayerCharacter>();
        animator = GetComponentInChildren<Animator>();
        enemyCharacter = GetComponent<EnemyCharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        SetSlider();
        SetCanvas();

        // Debugging
        if (isAttacking)
        {
            SetAttackState(true);
        }

        ManageRotation();
    }

    private void SetCanvas()
    {
        if (playerCharacter.currentHP <= 0)
        {
            canvas.gameObject.SetActive(false);
            return;
        }

        if (isAttacking)
        {
            canvas.gameObject.SetActive(true);
        }
        else
        {
            canvas.gameObject.SetActive(false);
        }
    }

    private void SetSlider()
    {
        if (isAttacking)
        {
            healthSlider.gameObject.SetActive(true);
        }
        else
        {
            healthSlider.gameObject.SetActive(false);
        }

        if (enemyCharacter.currentHP <= 0)
        {
            healthSlider.gameObject.SetActive(false);
            return;
        }

        healthSlider.maxValue = enemyCharacter.maxHP;
        healthSlider.value = enemyCharacter.currentHP;
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
        spells[randInt].Cast(enemyCharacter, transform.up);
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
