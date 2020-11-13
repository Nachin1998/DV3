using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    public enum EnemyState
    {
        Wandering,
        Chasing
    }
    public EnemyState enemyState = EnemyState.Wandering;

    public float health = 100f;
    public float damage = 10f;
    public float speed = 10f;
    public float attackDistance = 3f;
    public float attackSpeedRate = 2f;

    [Space]

    public ParticleSystem explosion;
    protected Player playerTarget;     
    protected NavMeshAgent agent;

    public bool isDead { get { return health <= 0; } }

    protected float maxAttackSpeedRate;

    // Update is called once per frame
    protected void InitBaseEnemy()
    {
        agent = GetComponent<NavMeshAgent>();

        maxAttackSpeedRate = attackSpeedRate;
        agent.speed = speed;
        playerTarget = FindObjectOfType<Player>();        
    }

    protected void UpdateBaseEnemy()
    {
        if (agent.speed >= speed)
        {
            agent.speed = speed;
        }
        else
        {
            agent.speed += Time.deltaTime;
        }

        if (isDead)
        {
            Die();
        }        
    }

    public virtual void ChasePlayer()
    {
        if (isDead)
        {
            return;
        }

        Vector3 distaceToAttack = playerTarget.transform.position - transform.position;
        if (playerTarget)
        {
            agent.SetDestination(playerTarget.transform.position - distaceToAttack.normalized);
        }

        if (playerTarget.isDead)
        {
            return;
        }

        if (Vector3.Distance(transform.position, playerTarget.transform.position) <= attackDistance)
        {
            if (attackSpeedRate <= 0)
            {
                AttackTarget();
            }
            else
            {
                attackSpeedRate -= Time.deltaTime;
            }
        }
        else
        {
            attackSpeedRate = 0;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (agent.speed > speed / 2)
        {
            agent.speed -= 1;
        }
    }

    public virtual void AttackTarget()
    {
        playerTarget.TakeDamage(damage);
        attackSpeedRate = maxAttackSpeedRate;
    }

    public void Die()
    {
        GameObject explosionGO = Instantiate(explosion.gameObject, transform.position, Quaternion.identity);
        Destroy(explosionGO, 2);
        gameObject.SetActive(false);
        Destroy(gameObject, 2.1f);
    }
}
