using UnityEngine;

public class ZombieRange : EnemyClass
{
    [Header("Ranged Attack")]
    public GameObject projectilePrefab;
    public Transform shootPoint;

    public float projectileDamage = 10f;
    public float attackCooldown = 2f;

    public float attackRange = 6f;
    public float safeDistance = 5f;

    float attackTimer;
    bool reachedDefenseLine = false;

    protected override void FixedUpdate()
    {
        if (target == null)
            return;

        float distance = Vector2.Distance(transform.position, target.position);

        if (GameManager.Instance.CurrentMode == GameMode.BaseDefense)
        {
            HandleBaseDefense(distance);
        }
        else
        {
            HandleTowerDefense();
        }

        HandleAttack(distance);
    }

    void HandleBaseDefense(float distance)
    {
        if (agent == null || !agent.enabled)
            return;

        if (distance > attackRange)
        {
            agent.SetDestination(target.position);
        }
        else if (distance < safeDistance)
        {
            Vector3 dir = (transform.position - target.position).normalized;
            Vector3 fleePos = transform.position + dir * 3f;

            agent.SetDestination(fleePos);
        }
        else
        {
            agent.ResetPath();
        }
    }

    void HandleTowerDefense()
    {
        if (reachedDefenseLine)
        {
            rb2D.linearVelocity = Vector2.zero;
        }
        else
        {
            rb2D.linearVelocity = Vector2.left * moveSpeed;
        }
    }

    void HandleAttack(float distance)
    {
        attackTimer -= Time.fixedDeltaTime;

        if (GameManager.Instance.CurrentMode == GameMode.BaseDefense)
        {
            if (distance >= safeDistance && distance <= attackRange)
            {
                TryAttack();
            }
        }
        else
        {
            if (reachedDefenseLine)
            {
                TryAttack();
            }
        }
    }

    void TryAttack()
    {
        if (attackTimer <= 0)
        {
            Attack();
            attackTimer = attackCooldown;
        }
    }

    public override void Attack()
    {
        if (projectilePrefab == null || shootPoint == null)
            return;

        GameObject proj = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

        EnemyProjectile projectile = proj.GetComponent<EnemyProjectile>();

        if (projectile != null)
        {
            if (GameManager.Instance.CurrentMode == GameMode.BaseDefense)
            {
                projectile.Initialize(target.position, projectileDamage);
            }
            else
            {
                projectile.InitializeForward(projectileDamage);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (GameManager.Instance.CurrentMode != GameMode.TowerDefense)
            return;

        if (col.CompareTag("DefenseLine"))
        {
            reachedDefenseLine = true;
        }
    }
}
