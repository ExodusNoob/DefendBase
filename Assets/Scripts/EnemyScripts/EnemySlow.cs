using UnityEngine;

public class EnemySlow : EnemyClass
{
    public float damage = 10f;
    public float attackRange = 1f;
    public float attackCooldown = 3f;
    Collider2D myCollider;

    float lastAttackTime;
    protected override void Awake()
    {
        base.Awake();
        moveSpeed = 3f; // Más rápido
        myCollider = GetComponent<Collider2D>();
    }
    protected override void FixedUpdate()
    {
        if (target == null) return;

        // Solo usamos el collider cuando atacamos el muro
        if (GameManager.Instance.CurrentMode == GameMode.TowerDefense)
        {
            if (targetCollider == null) return;

            float distance = targetCollider.Distance(myCollider).distance;
            if (distance <= attackRange)
            {
                if (rb2D != null)
                    rb2D.linearVelocity = Vector2.zero;

                Attack();
                return;
            }
        }

        Move();
    }
    public override void Attack()
    {
        if (Time.time < lastAttackTime + attackCooldown) return;

        lastAttackTime = Time.time;

        IDamageable damageable = target.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }

        Debug.Log("enemigo atacó el muro");
    }
}