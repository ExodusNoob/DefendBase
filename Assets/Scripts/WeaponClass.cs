using UnityEngine;
using UnityEngine.InputSystem;

public abstract class WeaponClass : MonoBehaviour
{
    [Header("Weapon")]
    public string weaponName;
    public float damage;
    public float attackRate;

    protected float lastAttackTime;
    float lastUseTime;

    [SerializeField] SpriteRenderer playerRenderer;
    SpriteRenderer weaponRenderer;

    float idleDelay = 4f;
    protected bool isWeaponActive = false;

    protected virtual void Awake()
    {
        weaponRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        HandleInput();
        CheckIdleTimeout();
    }
    void LateUpdate()
    {
        UpdateSorting();
    }
    void HandleInput()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            isWeaponActive = true;
            lastUseTime = Time.time;

            TryAttack();
        }
    }

    void TryAttack()
    {
        if (Time.time < lastAttackTime + attackRate)
            return;

        lastAttackTime = Time.time;

        Attack();
    }
    void CheckIdleTimeout()
    {
        if (isWeaponActive && Time.time > lastUseTime + idleDelay)
        {
            isWeaponActive = false;

            transform.rotation = Quaternion.identity;
        }
    }
    void UpdateSorting()
    {
        if (isWeaponActive)
        {
            weaponRenderer.sortingOrder = playerRenderer.sortingOrder + 1;
        }
        else
        {
            weaponRenderer.sortingOrder = playerRenderer.sortingOrder - 1;
        }
    }

    protected abstract void Attack();
}