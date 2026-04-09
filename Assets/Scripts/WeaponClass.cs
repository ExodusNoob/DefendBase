using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class WeaponClass : MonoBehaviour
{
    [SerializeField] private ControlPlayer playerControl;

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

    public void SetPlayerRenderer(SpriteRenderer renderer)
    {
        playerRenderer = renderer;
    }
    public void Initialize(SpriteRenderer renderer, ControlPlayer control)
    {
        playerRenderer = renderer;
        playerControl = control;
        weaponRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void Awake()
    {
        weaponRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        if(isWeaponActive)
            HandleFlipWhileAttacking();

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

            if (playerControl != null)
                playerControl.ClearForceFlip();
        }
    }
    void UpdateSorting()
    {
        if (weaponRenderer == null || playerRenderer == null)
            return;

        if (isWeaponActive)
        {
            weaponRenderer.sortingOrder = playerRenderer.sortingOrder + 1;
        }
        else
        {
            weaponRenderer.sortingOrder = playerRenderer.sortingOrder - 1;
        }
    }
    protected virtual void OnEnable()
    {
        isWeaponActive = false;
    }
    protected virtual void OnDisable()
    {
        ResetWeaponState();

        if (playerControl != null)
            playerControl.ClearForceFlip();
    }
    protected virtual void ResetWeaponState()
    {
        //vacio o cosas genericas que comparten las armas
        if (playerControl != null)
        {
            playerControl.ClearForceFlip();
        }
    }
    void HandleFlipWhileAttacking()
    {
        if (playerControl == null) return;
        if (!isWeaponActive) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePos.z = 0f;

        bool mouseLeft = mousePos.x < playerControl.transform.position.x;

        playerControl.SetForcedFlip(mouseLeft);
    }

    protected abstract void Attack();
}