using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float health = 100f;
    public float damage = 10f;
    public float attackDistance = 3f;
    public float attackSpeedRate = 2f;
    public bool isDead = false;

    public Explosion explosion;
    [Space]

    public NavMeshAgent agent;

    Player playerTarget;
    Vector3 distaceToAttack;
    float maxSpeedRate;

    // Update is called once per frame
    private void Start()    
    {
        maxSpeedRate = attackSpeedRate;
        playerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        distaceToAttack = playerTarget.transform.position - transform.position;
        if (health <= 0)
        {
            isDead = true;
        }

        if (isDead)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if (playerTarget && !isDead)
        {
            agent.SetDestination(playerTarget.transform.position - distaceToAttack.normalized);
            //transform.LookAt(target.transform);
        }

        if (!isDead && agent.remainingDistance <= attackDistance)
        {
            AttackTarget();
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    void AttackTarget()
    {
        if (attackSpeedRate <= 0)
        {
            playerTarget.TakeDamage(damage);
            attackSpeedRate = maxSpeedRate;
        }
        else
        {
            attackSpeedRate -= Time.deltaTime;
        }
    }
}
