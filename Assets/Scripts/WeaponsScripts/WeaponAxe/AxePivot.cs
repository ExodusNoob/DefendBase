using UnityEngine;
using UnityEngine.InputSystem;

public class AxePivot : MonoBehaviour
{
    [SerializeField] WeaponAxe axeWeapon;

    [SerializeField] float returnSpeed = 10f;

    Camera cam;
    Vector3 baseScale;

    private void Awake()
    {
        cam = Camera.main;

        baseScale = transform.localScale;
    }
    void Update()
    {
        if (axeWeapon == null) return;

        if (axeWeapon.IsAttacking())
            RotateToMouse();
        else
            ReturnToDefault();
    }

    void RotateToMouse()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePos.z = 0f;

        Vector2 direction = mousePos - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);

        // FLIP LIMPIO

        if (angle > 90 || angle < -90)
            transform.localScale = new Vector3(-Mathf.Abs(baseScale.x),baseScale.y, baseScale.z);
        else
            transform.localScale = new Vector3(Mathf.Abs(baseScale.x), baseScale.y, baseScale.z);
    }

    void ReturnToDefault()
    {
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.identity,
            returnSpeed * Time.deltaTime
        );
    }
    void OnDisable()
    {
        ResetPivot();
    }
    void OnEnable()
    {
        ResetPivot();
    }
    void ResetPivot()
    {
        transform.localRotation = Quaternion.identity;
        Vector3 scale = transform.localScale;
        transform.localScale = baseScale;
    }
}