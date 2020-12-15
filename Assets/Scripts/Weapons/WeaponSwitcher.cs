using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public int selectedWeaponIndex = 0;
    void Start()
    {
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        if (Pause.gameIsPaused)
        {
            return;
        }
        ScrollWheelSelection();
        NumPadSelection();
    }

    void NumPadSelection()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeaponIndex = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedWeaponIndex = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedWeaponIndex = 2;
        }

        SelectWeapon();
    }

    void ScrollWheelSelection()
    {
        int previousSelectedWeaponIndex = selectedWeaponIndex;

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (selectedWeaponIndex >= transform.childCount - 1)
            {
                selectedWeaponIndex = 0;
            }
            else
            {
                selectedWeaponIndex++;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (selectedWeaponIndex <= 0)
            {
                selectedWeaponIndex = transform.childCount - 1;
            }
            else
            {
                selectedWeaponIndex--;
            }
        }

        if (previousSelectedWeaponIndex != selectedWeaponIndex)
        {
            AkSoundEngine.PostEvent("player_gunchange", gameObject);
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int weaponIndex = 0;

        foreach(Transform weapon in transform)
        {
            if(weaponIndex == selectedWeaponIndex)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            weaponIndex++;
        }
    }
}
