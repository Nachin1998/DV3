using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponUI : MonoBehaviour
{
    public BaseWeapon bw;
    public TextMeshProUGUI ammoText;

    // Update is called once per frame
    void Update()
    {
        ammoText.text = bw.ammoInWeapon.ToString() + " / " + bw.maxAmmo.ToString();
    }
}
