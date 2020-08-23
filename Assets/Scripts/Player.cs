using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float health = 100f;
    public bool isDead = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }

        if(health <= 0)
        {
            isDead = true;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
