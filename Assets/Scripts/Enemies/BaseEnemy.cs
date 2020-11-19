using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Wandering,
        Chasing,
        Attacking, 
        Dead
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
    protected Animator anim;
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

        //AkSoundEngine.PostEvent("bear_dead", gameObject);
    }

    protected void UpdateBaseEnemy()
    {
        if (playerTarget.isDead)
        {
            enemyState = EnemyState.Idle;
        }

        if (isDead)
        {
            enemyState = EnemyState.Dead;
        }        

        switch (enemyState)
        {
            case EnemyState.Idle:
                agent.speed = 0;

                anim.SetBool("startedWalking", false);
                anim.SetBool("isWalking", false);
                anim.SetBool("startedRunning", false);
                anim.SetBool("isRunning", false);
                anim.SetBool("isAttacking", false);
                break;

            case EnemyState.Wandering:
                if (Vector3.Distance(transform.position, playerTarget.transform.position) <= sightRange)
                {
                    enemyState = EnemyState.Chasing;
                }
                Wander();                              
                break;

            case EnemyState.Chasing:
                if (Vector3.Distance(transform.position, playerTarget.transform.position) <= attackDistance)
                {
                    enemyState = EnemyState.Attacking;
                }
                ChasePlayer();                
                break;

            case EnemyState.Attacking:
                if (Vector3.Distance(transform.position, playerTarget.transform.position) >= attackDistance)
                {
                    enemyState = EnemyState.Chasing;
                }
                Attack();                
                break;

            case EnemyState.Dead:
                StartCoroutine(Die(2f));
                break;

            default:
                break;
        }   
    }

    public virtual void Wander()
    {  
        agent.speed = wanderSpeed;

        timer += Time.deltaTime;

        anim.SetBool("startedRunning", false);
        anim.SetBool("isRunning", false);
        anim.SetBool("startedWalking", true);
        anim.SetBool("isWalking", true);

        if (timer >= wanderTimer)
        {
            Vector3 newPos;
            if (areaToWander == null)
            {
                newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            }
            else
            {
                newPos = RandomNavSphere(areaToWander.position, wanderRadius, -1);
            }
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    public virtual void ChasePlayer()
    {
        agent.speed = chasingSpeed;

        anim.SetBool("startedRunning", true);
        anim.SetBool("isRunning", true);

        Vector3 distaceToAttack = playerTarget.transform.position - transform.position;
        if (playerTarget)
        {
            agent.SetDestination(playerTarget.transform.position - distaceToAttack.normalized);
        }
    }

    public virtual void Attack()
    {
        attackSpeedRate -= Time.deltaTime;

        anim.SetBool("startedWalking", false);
        anim.SetBool("isWalking", false);
        anim.SetBool("startedRunning", false);
        anim.SetBool("isRunning", false);

        if (attackSpeedRate <= 0)
        {
            StartCoroutine(AttackTarget(2));
            attackSpeedRate = maxAttackSpeedRate;
        }
    }

    public Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
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

    public virtual IEnumerator AttackTarget(float duration)
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
