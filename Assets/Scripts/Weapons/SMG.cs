using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMG : BaseWeapon
{
    // Start is called before the first frame update
    float timer;

    void Start()
    {
        InitWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAmmo();

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!isReloading && !isOutOfClips && canReload)
            {
                StartCoroutine(Reload(2.8f));
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (!isReloading && !isOutOfAmmo)
            {
                timer += Time.deltaTime;
                if (timer >= fireRate)
                {
                    isShooting = true;
                }
                else
                {
                    isShooting = false;
                }

                if (isShooting)
                {
                    Fire();
                    timer = 0;
                }
            }
            else
            {
                timer = 0;
            }
        }
    }
}
