//using System.Diagnostics;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField]
    private ControlPlayer playerControl;

    [SerializeField]
    private SpriteRenderer playerRenderer;

    [Header("Slots")]
    [SerializeField] private Transform[] weaponSlots; //0,1,2

    private GameObject[] equippedWeapons = new GameObject[3];

    int currentWeaponIndex = 0;

    void Awake()
    {
        if (playerRenderer == null)
        {
            playerRenderer = GetComponentInChildren<SpriteRenderer>();

            if (playerRenderer == null)
            {
                Debug.Log("No se encontro SpriteRenderer en el Player dx");
            }
        }
    }
    void Update()
    {
    }

    //TOGGLE EQUIP
    public void ToggleEquipWeapon(GameObject weaponPrefab)
    {
        Debug.Log("llegamos, verdad?");

        if (weaponPrefab == null)
        {
            Debug.LogError("weaponPrefab es NULL");
            return;
        }

        var prefabWeapon = weaponPrefab.GetComponentInChildren<WeaponClass>();

        if (prefabWeapon == null)
        {
            Debug.LogError($"El prefab {weaponPrefab.name} no tiene WeaponClass");
            return;
        }

        // 1. ¿ya está equipada?
        for (int i = 0; i < equippedWeapons.Length; i++)
        {
            if (equippedWeapons[i] != null)
            {
                var equipped = equippedWeapons[i].GetComponentInChildren<WeaponClass>();

                if (equipped == null) continue;

                if (equipped.GetType() == prefabWeapon.GetType())
                {
                    RemoveWeapon(i);
                    return;
                }
            }
        }

        // 2. buscar slot vacío
        for (int i = 0; i < equippedWeapons.Length; i++)
        {
            if (equippedWeapons[i] == null)
            {
                EquipWeaponInSlot(i, weaponPrefab);
                return;
            }
        }

        Debug.Log("todos los slots están ocupados");
    }

    //Equipar
    public void EquipWeaponInSlot(int slotIndex, GameObject weaponPrefab)
    {
        GameObject newWeapon = Instantiate(weaponPrefab, weaponSlots[slotIndex]);

        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localRotation = Quaternion.identity;

        equippedWeapons[slotIndex] = newWeapon;

        //magic

        var weapon = newWeapon.GetComponentInChildren<WeaponClass>();

        if (weapon != null)
        {
            Debug.Log("sacamos el player renderer?");
            weapon.Initialize(playerRenderer, playerControl);
        }

        newWeapon.SetActive(false);

        //si es la primera arma, activemosla :p

        if (GetFirstEquippedIndex() == slotIndex)
        {
            SwitchWeapon(slotIndex);
        }
    }

    int GetFirstEquippedIndex()
    {
        for (int i = 0; i < equippedWeapons.Length; i++)
        {
            if (equippedWeapons[i] != null)
                return i;
        }
        return -1;
    }

    //Remover
    public void RemoveWeapon(int SlotIndex)
    {
        if (equippedWeapons[SlotIndex] != null)
        {
            Destroy(equippedWeapons[SlotIndex]);
            equippedWeapons[SlotIndex] = null;
        }
    }

    //Cambiar arma
    
    public void SwitchWeapon(int index)
    {
        if (index < 0 || index >= equippedWeapons.Length)
            return;

        currentWeaponIndex = index;

        for (int i = 0; i < equippedWeapons.Length; i++)
        {
            if (equippedWeapons[i] != null)
            {
                equippedWeapons[i].SetActive(i == index);
            }
        }
    }
    public bool IsWeaponEquipped(GameObject weaponPrefab)
    {
        var prefabWeapon = weaponPrefab.GetComponentInChildren<WeaponClass>();

        foreach (var weapon in equippedWeapons)
        {
            if (weapon == null) continue;

            var equipped = weapon.GetComponentInChildren<WeaponClass>();

            if (equipped.GetType() == prefabWeapon.GetType())
            {
                return true;
            }
        }
        return false;
    }
    public bool HasAnyWeaponEquipped()
    {
        foreach(var weapon in equippedWeapons)
        {
            if (weapon != null)
                return true;
        }
        return false;
    }
    //void HandleWeaponScroll()
    //{
    //    float scroll = playerControl.GetScroll();
    //
    //    if (scroll > 0f)
    //    {
    //        SwitchToNextWeapon();
    //    }
    //    else if (scroll < 0f)
    //    {
    //        SwitchToPreviousWeapon();
    //    }
    //}
    void SwitchToNextWeapon()
    {
        int startIndex = currentWeaponIndex;

        do
        {
            currentWeaponIndex = (currentWeaponIndex + 1) % equippedWeapons.Length;

            if (equippedWeapons[currentWeaponIndex] != null)
            {
                SwitchWeapon(currentWeaponIndex);
                return;
            }
        }
        while (currentWeaponIndex != startIndex);
    }
    void SwitchToPreviousWeapon()
    {
        int startIndex = currentWeaponIndex;

        do
        {
            currentWeaponIndex--;

            if (currentWeaponIndex < 0)
                currentWeaponIndex = equippedWeapons.Length - 1;

            if (equippedWeapons[currentWeaponIndex] != null)
            {
                SwitchWeapon(currentWeaponIndex);
                return;
            }
        }
        while (currentWeaponIndex != startIndex);
    }
    private void OnEnable()
    {
        playerControl.OnScroll += HandleScroll;
    }
    private void OnDisable()
    {
        playerControl.OnScroll -= HandleScroll;
    }
    void HandleScroll(float scroll)
    {
        if (scroll > 0)
            SwitchToNextWeapon();
        else if(scroll < 0)
            SwitchToPreviousWeapon();
    }
}