using UnityEngine;

public class ObjectToProtect : Entity
{
    [Header("Player Reference")]
    private Transform player;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void Update()
    {
        HandleFlip();
    }

    override protected void HandleFlip()
    {
        if(player == null) return;

        if(player.position.x > transform.position.x && !facingRight)
            Flip();
        else if(player.position.x < transform.position.x && facingRight)
            Flip();
    }

    protected override void OnHealthChanged(int currentHealth)
    {
        UI.Instance.UpdateGirlHealth(currentHealth);
    }

    protected override void Die()
    {
        base.Die();
        UI.Instance.EnableGameOverUI();
    }
}
