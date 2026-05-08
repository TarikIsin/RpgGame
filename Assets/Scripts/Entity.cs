using System;
using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator animator;
    protected Collider2D col;
    protected SpriteRenderer sr;

    [Header("Health")]
    [SerializeField] protected int maxHealth = 1;
    [SerializeField] protected int currentHealth;
    [SerializeField] private Material damageMaterial;
    [SerializeField] private float damageFeedbackDuration = 0.1f;
    private Coroutine damageFeedbackCoroutine;

    [Header("Attack")]
    [SerializeField] protected float attackRadius;
    [SerializeField] protected LayerMask whatIsTarget;
    [SerializeField] protected Transform attackPoint;

    [Header("Collision")]
    [SerializeField] private float groundCheckDistance = 1.4f;
    protected bool isGrounded;
    [SerializeField] private LayerMask groundLayer;

    //Facing direction details
    protected int facingDir = 1;
    protected bool canMove = true;
    protected bool facingRight = true;


    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        animator = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();

        currentHealth = maxHealth;
        OnHealthChanged(currentHealth);
    }

    protected virtual void Update()
    {
        HandleCollision();
        HandleMovement();
        HandleAnimations();
        HandleFlip();
    }

    public void DamageTargets()
    {
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, whatIsTarget);
        foreach (Collider2D enemy in enemyColliders)
        {
            Entity entityTarget = enemy.GetComponent<Entity>();
            entityTarget.TakeDamage();
        }
    }

    private void TakeDamage()
    {
        currentHealth -= 1;
        PlayDamageFeedback();
        OnHealthChanged(currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    protected virtual void OnHealthChanged(int currentHealth)
    {
        
    }

    protected virtual void Die()
    {
        animator.enabled = false;
        col.enabled = false;

        rb.gravityScale = 12;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 15);

        Destroy(gameObject, 3f);
    }

    private void PlayDamageFeedback()
    {
        if (damageFeedbackCoroutine != null)
            StopCoroutine(damageFeedbackCoroutine);

        StartCoroutine(DamageFeedbackCo());
    }

    private IEnumerator DamageFeedbackCo()
    {
        Material originalMat = sr.material;
        sr.material = damageMaterial;
        
        yield return new WaitForSeconds(damageFeedbackDuration);

        sr.material = originalMat;
    }

    public virtual void EnableMovement(bool enable)
    {
        canMove = enable;
    }

    protected virtual void HandleAnimations()
    {
        animator.SetFloat("xVelocity", rb.linearVelocity.x);
    }

    protected virtual void HandleAttack()
    {
        if(isGrounded)
            animator.SetTrigger("attack");
    }

    protected virtual void HandleMovement()
    {
    }

    protected virtual void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
    }

    protected virtual void HandleFlip()
    {
        if (rb.linearVelocity.x > 0 && facingRight == false)
        {
            Flip();
        }
        else if (rb.linearVelocity.x < 0 && facingRight == true)
        {
            Flip();
        }
    }
    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        facingDir *= -1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));
        
        if(attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}