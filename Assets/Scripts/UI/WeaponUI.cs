using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponUI : MonoBehaviour
{
    public List <BaseWeapon> bws;
    public TextMeshProUGUI ammoText;

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < bws.Count; i++)
        {
            if (bws[i].isActiveAndEnabled)
            {
                ammoText.text = bws[i].ammoInWeapon.ToString() + " / " + bws[i].maxAmmo.ToString();
            }
        }
    }
}
