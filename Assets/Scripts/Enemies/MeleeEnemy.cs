using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : BaseEnemy
{
    void Start()    
    {
        InitBaseEnemy();      
    }

    void Update()
    {
        UpdateBaseEnemy();
        AkSoundEngine.PostEvent("bear_dead", gameObject);
    }
}
