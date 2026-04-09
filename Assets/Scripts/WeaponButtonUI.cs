using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponButtonUI : MonoBehaviour
{
    private GameObject weaponPrefab;
    private PlayerWeaponManager weaponManager;

    [SerializeField] private Image backGround;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nametext;

    [Header("Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color equippedColor = Color.green;

    void Start()
    {
        if (backGround == null) Debug.LogError("BackGround no asignado");
        if (nametext == null) Debug.LogError("NameText no asignado");
    }
    public void SetWeapon(GameObject weapon)
    {
        weaponPrefab = weapon;

        WeaponClass wc = weapon.GetComponentInChildren<WeaponClass>();

        if (wc != null && nametext != null)
        {
            nametext.text = wc.weaponName;
        }
        else
        {
            Debug.LogWarning($"No se encontró WeaponClass en {weapon.name}");
        }
    }

    public void SetWeaponManager(PlayerWeaponManager manager)
    {
        weaponManager = manager;
    }

    public void SetIcon(Sprite sprite)
    {
        if(icon != null)
        icon.sprite = sprite;
    }

    public void OnClick()
    {
        Debug.Log("Click en botón");

        if (weaponManager == null)
        {
            Debug.LogError("weaponManager es NULL");
            return;
        }

        if (weaponPrefab == null)
        {
            Debug.LogError("weaponPrefab es NULL");
            return;
        }

        weaponManager.ToggleEquipWeapon(weaponPrefab);

        UpdateVisual();
    }

    void Update()
    {
        UpdateVisual();
    }
    
    void UpdateVisual()
    {
        if (weaponManager == null || weaponPrefab == null)
            return;

        bool isEquipped = weaponManager.IsWeaponEquipped(weaponPrefab);

        if (backGround != null)
        {
            backGround.color = isEquipped ? equippedColor : normalColor;
        }
    }
}