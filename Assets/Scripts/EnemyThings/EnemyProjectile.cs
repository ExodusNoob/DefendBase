using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 10f;

    private float damage;
    private Vector2 targetPosition;
    private Vector2 direction;
    private bool hasTarget;

    public void Initialize(Vector2 targetPos, float dmg)
    {
        targetPosition = targetPos;
        damage = dmg;

        direction = (targetPosition - (Vector2)transform.position).normalized;
        hasTarget = true;


    }
    public void InitializeForward(float dmg)
    {
        damage = dmg;
        direction = Vector2.left;
        hasTarget = false;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        if (hasTarget)
        {
            float distance = Vector2.Distance(transform.position, targetPosition);

            if (distance < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Defense"))
            return;

        IDamageable target = collision.GetComponent<IDamageable>();

        if (target != null)
        {
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
