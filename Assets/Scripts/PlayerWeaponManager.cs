using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    [Header("Setup")]
    public Transform weaponPivot;

    [Header("Available Weapons")]
    public GameObject[] weaponPrefabs; // todas las armas disponibles

    GameObject[] equippedWeapons = new GameObject[3];

    int currentWeaponIndex = 0;

    void Start()
    {
        // Ejemplo inicial (puedes cambiarlo luego por UI)
        EquipWeaponInSlot(0, weaponPrefabs[0]); // Pistola
        EquipWeaponInSlot(1, weaponPrefabs[1]); // Rifle
        // slot 2 vacío por ahora

        SwitchWeapon(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SwitchWeapon(0);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SwitchWeapon(1);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            SwitchWeapon(2);
    }

    //  EQUIPAR arma en un slot (reemplaza si ya hay una)
    public void EquipWeaponInSlot(int slotIndex, GameObject weaponPrefab)
    {
        if (slotIndex < 0 || slotIndex >= equippedWeapons.Length)
            return;

        // eliminar arma anterior
        if (equippedWeapons[slotIndex] != null)
        {
            Destroy(equippedWeapons[slotIndex]);
        }

        // instanciar nueva arma
        GameObject newWeapon = Instantiate(weaponPrefab, weaponPivot);

        equippedWeapons[slotIndex] = newWeapon;

        // desactivar por defecto
        newWeapon.SetActive(false);
    }

    //  CAMBIAR arma activa (1,2,3)
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
}
