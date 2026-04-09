using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponAxe : WeaponClass
{
    [Header("Axe Settings")]
    public GameObject hitbox;
    public AxeHitbox axeHitbox;

    Animator animator;

    [SerializeField]
    int comboStep = 0;
    float lastClickTime;
    float comboResetTime = 1f;

    [SerializeField]
    bool isAttacking = false; //  controla el flujo

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        if (hitbox != null)
            hitbox.SetActive(false);
        else
            Debug.LogError("Hitbox no asignado en WeaponAxe");

        if (axeHitbox == null)
            Debug.LogError("AxeHitbox no asignado en WeaponAxe");
    }
    protected override void ResetWeaponState()
    {
        isAttacking = false;
        comboStep = 0;

        if (hitbox != null)
            hitbox.SetActive(false);

        if (animator != null && animator.isActiveAndEnabled)
        {
            animator.Rebind();
            animator.Update(0f);

            //animator.ResetTrigger("Attack");
            //animator.SetInteger("Combo", 0);
        }
    }
    protected override void Attack()
    {
        // No permitir atacar hasta que termine animación
        if (isAttacking) return;

        axeHitbox.damage = damage;

        // Reset combo si pasó mucho tiempo
        if (Time.time - lastClickTime > comboResetTime)
        {
            comboStep = 0;
        }

        comboStep++;

        if (comboStep > 3)
            comboStep = 1;

        animator.SetInteger("Combo", comboStep);
        animator.SetTrigger("Attack");

        lastClickTime = Time.time;
        isAttacking = true;

    }
    public void EndAttack()
    {
        isAttacking = false;
    }
    public bool IsAttacking()
    {
        return isAttacking;
    }
    public void EnableHitbox()
    {
        hitbox.SetActive(true);
    }

    public void DisableHitbox()
    {
        hitbox.SetActive(false);
    }
}