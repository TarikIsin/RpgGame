using UnityEngine;

public class Enemy : Entity
{
    private bool playerDetected;

    [Header("Movement")]
    [SerializeField] protected float moveSpeed = 8f;
    [SerializeField] private float attackCooldown = 1.5f;

    private float lastAttackTime;

    protected override void Update()
    {
        base.Update();
        HandleAttack();
    }

    protected override void HandleAttack()
    {
        if (playerDetected && Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            animator.SetTrigger("attack");
        }
    }

    protected override void HandleMovement()
    {
        if (canMove)
            rb.linearVelocity = new Vector2(facingDir * moveSpeed, rb.linearVelocity.y);
        else
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    protected override void HandleCollision()
    {
        base.HandleCollision();
        playerDetected = Physics2D.OverlapCircle(attackPoint.position, attackRadius, whatIsTarget);
    }

    protected override void Die()
    {
        base.Die();
        UI.Instance.AddKillCount();
    }
}