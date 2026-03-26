using UnityEngine;

public class EnemyFast : EnemyClass
{
    public float damage = 10f;
    public float attackRange = 1.2f;
    public float attackCooldown = 1f;

    protected override void Awake()
    {
        base.Awake();
        moveSpeed = 8f; // Más rápido
    }

    public override void Attack()
    {
        
    }
}
