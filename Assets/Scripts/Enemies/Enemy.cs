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

    public GameObject explosion;

    NavMeshAgent agent;
    GameObject target;

    Player playerTarget;
    GameObject platformTarget;

    float maxSpeedRate;

    // Update is called once per frame
    private void Start()    
    {
        agent = GetComponent<NavMeshAgent>();

        maxSpeedRate = attackSpeedRate;

        switch (GameManager.Instance.gameMode)
        {
            case GameManager.GameMode.Survival:
                target = GameObject.FindGameObjectWithTag("Player");
                playerTarget = target.GetComponent<Player>();
                break;

            case GameManager.GameMode.HoldZone:
                GameObject[] platforms = GameObject.FindGameObjectsWithTag("CapturePoint");
                platformTarget = platforms[Random.Range(0, platforms.Length)];
                break;
            default:
                break;
        }        
    }

    void Update()
    {
        if (agent.speed >= 5)
        {
            agent.speed = 5;
        }
        else
        {
            agent.speed += Time.deltaTime;
        }

        switch (GameManager.Instance.gameMode)
        {
            case GameManager.GameMode.None:
                break;
            case GameManager.GameMode.Survival:
                ChasePlayer();
                break;
            case GameManager.GameMode.HoldZone:
                SearchZone();
                break;
            default:
                break;
        }
        
    }

    void ChasePlayer()
    {
        Vector3 distaceToAttack = playerTarget.transform.position - transform.position;
       

        if (health <= 0)
        {
            isDead = true;
        }

        if (isDead)
        {
            Die();
        }

        if (playerTarget && !isDead)
        {
            agent.SetDestination(playerTarget.transform.position - distaceToAttack.normalized);
            //transform.LookAt(target.transform);
        }

        if (playerTarget.isDead)
        {
            return;
        }

        if (!isDead && Vector3.Distance(transform.position, playerTarget.transform.position) <= attackDistance)
        {
            AttackTarget();
        }
    }

    void SearchZone()
    {
        Debug.Log("HOLD AREA");
        Vector3 distaceToPlatform = platformTarget.transform.position - transform.position;
        agent.SetDestination(platformTarget.transform.position - distaceToPlatform.normalized);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if(agent.speed > 3.5f)
        {
            agent.speed -= 1;
        }
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

    public void Die()
    {
        GameObject explosionGO = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(explosionGO, 2);
        gameObject.SetActive(false);
        Destroy(gameObject, 2.1f);
    }
}
