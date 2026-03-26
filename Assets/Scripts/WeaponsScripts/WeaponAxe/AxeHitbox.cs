using System.Collections.Generic;
using UnityEngine;

public class AxeHitbox : MonoBehaviour
{
    public float damage;

    // Para no golpear múltiples veces al mismo enemigo en un solo swing
    private HashSet<EnemyClass> hitEnemies = new HashSet<EnemyClass>();

    private void OnEnable()
    {
        hitEnemies.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHitbox enemyHitbox = collision.GetComponent<EnemyHitbox>();

        if (enemyHitbox == null) return;

        EnemyClass enemy = enemyHitbox.enemy;

        if (!hitEnemies.Contains(enemy))
        {
            enemyHitbox.TakeHit(damage);
            hitEnemies.Add(enemy);
        }
    }
}
