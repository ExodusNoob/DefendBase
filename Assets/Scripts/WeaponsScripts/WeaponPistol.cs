using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponPistol : WeaponClass
{
    [Header("Pistol")]
    [SerializeField] Proyectile projectilePrefab;
    [SerializeField] Transform shootPosition;
    [SerializeField] float speedAmmo = 15f;

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
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 direction = mousePos - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
