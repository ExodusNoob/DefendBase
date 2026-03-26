using System.Runtime.CompilerServices;
using UnityEngine;

public class Proyectile : MonoBehaviour
{
    public float TimeDestroy = 15;
    bool hasHit = false;

    private Rigidbody2D projectileRigidBody2D_;

    [SerializeField] private float speedProjectile;
    [SerializeField] private float damage;

    //PlantillaEnemy enemy;

    private void Awake()
    {
        projectileRigidBody2D_ = GetComponent<Rigidbody2D>();
    }

    //void Update()
    //{
    //    SetData(speedProjectile, damage);
    //}
    public void SetData(float speed, float dmg)
    {
        speedProjectile = speed;
        damage = dmg;

        // Lanza el proyectil hacia adelante
        LaunchProjectile(transform.right);
    }

    public void LaunchProjectile(Vector2 direction)
    {
        Debug.Log("dispare");
        projectileRigidBody2D_.linearVelocity = direction * speedProjectile;
        Destroy(gameObject, TimeDestroy);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;

        EnemyHitbox hitbox = other.GetComponentInParent<EnemyHitbox>();

        if (hitbox != null)
        {
            hasHit = true;

            hitbox.TakeHit(damage);

            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D()
    {
        Destroy(gameObject);
    }
}