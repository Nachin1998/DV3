using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class Enemy : MonoBehaviour
{
    public float health = 100f;
    public Explosion explosion;
    public bool isDead = false;

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            isDead = true;
        }

        if (isDead)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
