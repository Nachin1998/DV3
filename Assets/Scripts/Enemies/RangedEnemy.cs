using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : BaseEnemy
{
    // Start is called before the first frame update
    public Transform fireSpot;
    public Projectile projectile;

    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        if (agent.speed >= speed)
        {
            agent.speed = speed;
        }
        else
        {
            agent.speed += Time.deltaTime;
        }

        if (health <= 0)
        {
            isDead = true;
        }

        if (isDead)
        {
            Die();
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

    public override void ChasePlayer()
    {
        if (isDead)
        {
            return;
        }

        Vector3 distaceToAttack = playerTarget.transform.position - transform.position;

        if (playerTarget)
        {
            agent.SetDestination(playerTarget.transform.position - distaceToAttack.normalized);
            if (enemyHead.gameObject != null)
            {
                enemyHead.transform.LookAt(target.transform);
            }
        }

        if (playerTarget.isDead)
        {
            return;
        }

        if (Vector3.Distance(transform.position, playerTarget.transform.position) <= attackDistance)
        {
            if (attackSpeedRate <= 0)
            {
                Fire();
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

    void Fire()
    {
        Instantiate(projectile, fireSpot.position, fireSpot.rotation);
        attackSpeedRate = maxSpeedRate;
    }
}
