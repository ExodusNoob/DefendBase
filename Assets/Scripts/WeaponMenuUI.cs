using UnityEngine;

public class WeaponMenuUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform content; //Content del Scrollview
    [SerializeField] private GameObject weaponButtonPrefab;

    [Header("Weapon Available")]
    [SerializeField] private GameObject[] allWeapons; //prefab de las armas

    [Header("Reference")]
    [SerializeField] private PlayerWeaponManager weaponManager;

    void Start()
    {
        GenerateWeaponButtons();   
    }

    void GenerateWeaponButtons()
    {
        foreach (GameObject weapon in allWeapons)
        {
            GameObject buttonGO = Instantiate(weaponButtonPrefab, content);

            WeaponButtonUI button = buttonGO.GetComponent<WeaponButtonUI>();

            //asignamos prefab
            button.SetWeapon(weapon);

            button.SetWeaponManager(weaponManager);

            // SOLO si luego agregas icono
            // WeaponClass wc = weapon.GetComponent<WeaponClass>();
            // if (wc != null)
            // {
            //     button.SetIcon(wc.icon);
            // }

        }
    }
}
