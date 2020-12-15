using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    public Player player;
    public Image crosshair;
    public List <BaseWeapon> bws;
    public TextMeshProUGUI ammoText;

    // Update is called once per frame
    void Update()
    {
        if (player.isDead)
        {
            crosshair.gameObject.SetActive(false);
            ammoText.gameObject.SetActive(false);
            return;
        }

        for (int i = 0; i < bws.Count; i++)
        {
            if (bws[i].isActiveAndEnabled)
            {
                ammoText.text = bws[i].ammoInWeapon.ToString() + " / " + bws[i].maxAmmo.ToString();
            }
        }
    }
}
