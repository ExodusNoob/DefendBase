using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    public EnemyClass enemy;

    public float damageMultiplier = 1f;

    public void TakeHit(float damage)
    {
        Debug.Log("lo multiplicaste por " + damageMultiplier + " jajajaj");
        enemy.TakeDamage(damage * damageMultiplier);
    }
}