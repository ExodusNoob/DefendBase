using UnityEngine;
using UnityEngine.InputSystem;

public class AxePivot : MonoBehaviour
{
    [SerializeField] WeaponAxe axeWeapon;

    [SerializeField] float returnSpeed = 10f;

    void Update()
    {
        if (axeWeapon == null) return;

        if (axeWeapon.IsAttacking())
        {
            RotateToMouse();
        }
        else
        {
            ReturnToDefault();
        }
    }

    void RotateToMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePos.z = 0f;

        Vector2 direction = mousePos - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);

        // FLIP LIMPIO
        Vector3 scale = transform.localScale;

        if (angle > 90 || angle < -90)
            scale.y = -1;
        else
            scale.y = 1;

        transform.localScale = scale;
    }

    void ReturnToDefault()
    {
        Quaternion targetRotation = Quaternion.identity;

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            returnSpeed * Time.deltaTime
        );

        // RESET FLIP
        Vector3 scale = transform.localScale;
        scale.y = 1;
        transform.localScale = scale;
    }
}
