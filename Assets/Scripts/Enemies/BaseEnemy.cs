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
    public string[] enemySounds;
    protected float speakingTimer;

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
    protected bool isAttacking;
    public bool isDead { get { return health <= 0; } }

    protected float maxAttackSpeedRate;
    protected float timer;

    bool playedDeathSound = false;
    float stepSoundOffset = 0.6f;
    float movingTimer;

    // Update is called once per frame
    protected void InitBaseEnemy()
    {
        agent = GetComponent<NavMeshAgent>();

        maxAttackSpeedRate = attackSpeedRate;
        attackSpeedRate = 0;
        agent.speed = wanderSpeed;
        playerTarget = FindObjectOfType<Player>();

        anim = GetComponent<Animator>();
        timer = wanderTimer;
        movingTimer = stepSoundOffset;
    }

    protected void UpdateBaseEnemy()
    {
        if (playerTarget.isDead)
        {
            anim.SetBool("startedRunning", false);
            anim.SetBool("isRunning", false);
            anim.SetBool("isAttacking", false);
            enemyState = EnemyState.Wandering;
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
                Wander();                
                break;

            case EnemyState.Chasing:
                ChasePlayer();                
                break;

            case EnemyState.Attacking:
                Attack();                
                break;

            case EnemyState.Dead:
                StartCoroutine(Die(2f));
                break;

            default:
                break;
        }

        MakeSound();
    }

    public virtual void Wander()
    {
        if (Vector3.Distance(transform.position, playerTarget.transform.position) <= sightRange && !playerTarget.isDead)
        {
            enemyState = EnemyState.Chasing;
        }

        agent.speed = wanderSpeed;

        timer += Time.deltaTime;

        //anim.SetBool("startedRunning", false);
        //anim.SetBool("isRunning", false);
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
        if (Vector3.Distance(transform.position, playerTarget.transform.position) <= attackDistance)
        {
            enemyState = EnemyState.Attacking;
        }

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
        if (Vector3.Distance(transform.position, playerTarget.transform.position) >= attackDistance)
        {
            anim.SetBool("isAttacking", false);
            attackSpeedRate = 0;
            enemyState = EnemyState.Wandering;
            return;
        }

        attackSpeedRate -= Time.deltaTime;

        //anim.SetBool("startedWalking", false);
        //anim.SetBool("isWalking", false);
        //anim.SetBool("startedRunning", false);
        //anim.SetBool("isRunning", false);
        anim.SetBool("isAttacking", true);

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
        isAttacking = true;
        playerTarget.TakeDamage(damage);
        attackSpeedRate = maxAttackSpeedRate;

        yield return new WaitForSeconds(duration);
        isAttacking = false;
    }

    public void MakeSound()
    {
        speakingTimer += Time.deltaTime;

        for (int i = 0; i < enemySounds.Length; i++)
        {
            if(speakingTimer >= 10)
            {
                AkSoundEngine.PostEvent(enemySounds[Random.Range(0, enemySounds.Length)], gameObject);
                speakingTimer = 0;
            }
        }
    }

    IEnumerator Die(float duration)
    {
        agent.speed = 0;
        anim.SetBool("isDead", true);
        if (!playedDeathSound)
        {
            AkSoundEngine.PostEvent("bear_dead", gameObject);
            playedDeathSound = true;
        }
        yield return new WaitForSeconds(duration);

        GameObject explosionGO = Instantiate(explosion.gameObject, transform.position, Quaternion.identity);
        Destroy(explosionGO, 2);
        gameObject.SetActive(false);
        Destroy(gameObject, 2.1f);
    }
}