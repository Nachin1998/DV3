using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : BaseEnemy
{
    // Start is called before the first frame update

    public Transform fireSpot;
    public Projectile projectile;
    public float minimumDistanceFromTarget;
    void Start()
    {
        InitBaseEnemy();
    }

    void Update()
    {
        UpdateBaseEnemy();
    }

    public override void ChasePlayer()
    {
        if (!playerTarget.isDead)
        {
            anim.SetBool("startedRunning", true);
            anim.SetBool("isRunning", true);

            Vector3 direction = playerTarget.transform.position - transform.position;
            Debug.Log(direction.magnitude);
            if (direction.magnitude > 40)
            {
                agent.stoppingDistance = 40;
                agent.SetDestination(playerTarget.transform.position);
            }
        }
        else
        {
            anim.SetBool("startedWalking", false);
            anim.SetBool("isWalking", false);
        }
    }

    public override void Attack()
    {
        attackSpeedRate -= Time.deltaTime;        

        Vector3 direction = playerTarget.transform.position - transform.position;
        if (direction.magnitude < 20)
        {
            anim.SetBool("isAttacking", false);
            anim.SetBool("startedRunning", true);
            anim.SetBool("isRunning", true);
            agent.stoppingDistance = 0;
            agent.speed = 40;
            agent.SetDestination(agent.transform.position - direction * Time.deltaTime * agent.speed);
        }
        else
        {
            if(agent.velocity.magnitude == 0)
            {
                if (attackSpeedRate <= 0)
                {
                    anim.SetBool("startedWalking", false);
                    anim.SetBool("isWalking", false);
                    anim.SetBool("startedRunning", false);
                    anim.SetBool("isRunning", false);

                    StartCoroutine(AttackTarget(2));
                    attackSpeedRate = maxAttackSpeedRate;
                }
            }           
        }        
    }

    public override IEnumerator AttackTarget(float duration)
    {
        anim.SetBool("isAttacking", true);
        Instantiate(projectile, fireSpot.position, fireSpot.rotation);
        attackSpeedRate = maxAttackSpeedRate;

        yield return new WaitForSeconds(duration);
        anim.SetBool("isAttacking", false);
    }
}
