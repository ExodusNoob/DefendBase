using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponRifle : WeaponClass
{
    [Header("Rifle")]
    [SerializeField] Proyectile projectilePrefab;
    [SerializeField] Transform shootPosition;
    [SerializeField] float speedAmmo;

    bool isAttacking;

    protected override void Update()
    {
        base.Update();

        if (isWeaponActive)
            RotateWeapon();
    }

    protected override void Attack()
    {
        Proyectile bullet = Instantiate(
            projectilePrefab,
            shootPosition.position,
            transform.rotation
        );

        bullet.SetData(speedAmmo, damage);
    }

    void RotateWeapon()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        Vector3 direction = mousePosition - Camera.main.WorldToScreenPoint(transform.position);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}