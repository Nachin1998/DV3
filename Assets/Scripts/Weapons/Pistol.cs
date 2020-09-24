using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : BaseWeapon
{
    // Start is called before the first frame update

    // Update is called once per frame
    new void Start()
    {
        base.Start();
    }

    new void Update()
    {
        base.Update();

        if (Input.GetMouseButtonDown(0))
        {
            if (!isReloading && !isOutOfAmmo)
            {
                base.Fire();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!isReloading && !isOutOfClips && canReload)
            {
                StartCoroutine(Reload(2.8f));
            }
        }
    }
}
