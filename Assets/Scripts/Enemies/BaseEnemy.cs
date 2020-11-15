using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    public enum EnemyState
    {
        Wandering,
        Chasing,
        Attacking
    }

    [Header("Enemy Data")]
    public EnemyState enemyState = EnemyState.Wandering;

    public float health = 100f;
    public float damage = 10f;
    public float sightRange = 20f;
    [Header("Chasing")]
    public float attackDistance = 3f;
    public float attackSpeedRate = 2f;
    public float chasingSpeed = 10f;

    [Header("Wandering")]
    public float wanderRadius;
    public float wanderTimer;
    public float wanderSpeed = 7f;
    public Transform areaToWander;

    [Space]

    public ParticleSystem explosion;
    protected Player playerTarget;     
    protected NavMeshAgent agent;
    Animator anim;
    public bool isDead { get { return health <= 0; } }

    protected float maxAttackSpeedRate;
    protected float timer;

    // Update is called once per frame
    protected void InitBaseEnemy()
    {
        agent = GetComponent<NavMeshAgent>();

        maxAttackSpeedRate = attackSpeedRate;
        agent.speed = wanderSpeed;
        playerTarget = FindObjectOfType<Player>();

        anim = GetComponent<Animator>();
        timer = wanderTimer;
    }

    protected void UpdateBaseEnemy()
    {
       /* if (agent.velocity.magnitude != 0)// && enemyState != EnemyState.Chasing)
        {
            anim.SetBool("startedWalking", true);
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("startedWalking", false);
            anim.SetBool("isWalking", false);
        }*/

        if (isDead)
        {
            StartCoroutine(Die(2f));
            return;
        }

        switch (enemyState)
        {
            case EnemyState.Wandering:
                Wander();
                if (Vector3.Distance(transform.position, playerTarget.transform.position) <= sightRange)
                {
                    enemyState = EnemyState.Chasing;
                }                
                break;

            case EnemyState.Chasing:
                ChasePlayer();

                if (Vector3.Distance(transform.position, playerTarget.transform.position) <= attackDistance)
                {
                    enemyState = EnemyState.Attacking;
                }
                break;

            case EnemyState.Attacking:
                Attack();

                if (Vector3.Distance(transform.position, playerTarget.transform.position) >= attackDistance)
                {
                    enemyState = EnemyState.Wandering;
                }
                break;

            default:
                break;
        }   
    }

    public virtual void ChasePlayer()
    {
        agent.speed = chasingSpeed;

        Vector3 distaceToAttack = playerTarget.transform.position - transform.position;
        if (playerTarget)
        {
            agent.SetDestination(playerTarget.transform.position - distaceToAttack.normalized);
        }

        anim.SetBool("startedRunning", true);
        anim.SetBool("isRunning", true);

        if (playerTarget.isDead)
        {
            anim.SetBool("startedWalking", false);
            anim.SetBool("isWalking", false);
            return;
        }
    }

    public void Wander()
    {
        agent.speed = wanderSpeed;

        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(areaToWander.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }

        if (agent.velocity.magnitude != 0)
        {
            anim.SetBool("startedWalking", true);
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("startedWalking", false);
            anim.SetBool("isWalking", false);
        }
        anim.SetBool("startedRunning", false);
        anim.SetBool("isRunning", false);
    }

    public void Attack()
    {
        attackSpeedRate -= Time.deltaTime;

        anim.SetBool("startedRunning", false);
        anim.SetBool("isRunning", false);

        if (attackSpeedRate <= 0)
        {
            StartCoroutine(AttackTarget(2));
            attackSpeedRate = maxAttackSpeedRate;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    private void ChangeState()
    {
        int intState = (int)enemyState;
        intState++;
        intState = intState % ((int)EnemyState.Chasing);
        enemyState = (EnemyState)intState;
    }

    IEnumerator AttackTarget(float duration)
    {
        anim.SetBool("isAttacking", true);
        playerTarget.TakeDamage(damage);
        attackSpeedRate = maxAttackSpeedRate;

        yield return new WaitForSeconds(duration);

        anim.SetBool("isAttacking", false);
    }

    IEnumerator Die(float duration)
    {
        agent.speed = 0;
        anim.SetBool("isDead", true);

        yield return new WaitForSeconds(duration);

        GameObject explosionGO = Instantiate(explosion.gameObject, transform.position, Quaternion.identity);
        Destroy(explosionGO, 2);
        gameObject.SetActive(false);
        Destroy(gameObject, 2.1f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(areaToWander.position, wanderRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
