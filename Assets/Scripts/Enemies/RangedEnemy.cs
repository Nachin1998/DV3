using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : BaseEnemy
{
    // Start is called before the first frame update

    public Transform fireSpot;
    public Projectile projectile;
    public float minimumDistanceFromTarget;
    new void Start()
    {
        base.Start();
    }

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
        
        Vector3 direction = playerTarget.transform.position - transform.position;
        Vector3 direction2 = transform.position - playerTarget.transform.position;

        Debug.Log(direction.magnitude);
        if (playerTarget)
        {
            if (direction.magnitude > 40)
            {
                agent.stoppingDistance = 40;
                agent.SetDestination(playerTarget.transform.position);
            }
            else if (direction.magnitude < 20)
            {                
                agent.stoppingDistance = 0;
                agent.speed = 40;
                agent.SetDestination(agent.transform.position -direction * Time.deltaTime * agent.speed);
            }

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

    /*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DVJ02.Clase08
{
    public class EnemyAIFSM : MonoBehaviour //, IPointerEnterHandler
    {
        public enum EnemyState
        {
            Idle, //0
            GoingToTarget, //1
            GoAway, //2
            Last,
        }

        [SerializeField] private EnemyState state;

        public float speed = 10;
        public float distanceToStop = 2;
        public float distanceToRestart = 10;

        public Transform target;

        private float t;

        private void Update()
        {
            t += Time.deltaTime;
            switch (state)
            {
                case EnemyState.Idle:
                    if (t > 2)
                    {
                        NextState();
                    }
                    break;
                case EnemyState.GoingToTarget:
                    Vector3 dir = target.position - transform.position;
                    transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World); //, Space.World);
                    if (Vector3.Distance(transform.position, target.position) < distanceToStop)
                        NextState();
                    break;
                case EnemyState.GoAway:
                    Vector3 dir02 = transform.position - target.position;
                    transform.Translate(dir02.normalized * speed * Time.deltaTime, Space.World);
                    if (Vector3.Distance(transform.position, target.position) > distanceToRestart)
                        NextState();
                    break;
            }
        }

        private void NextState()
        {
            t = 0;
            int intState = (int)state;
            intState++;
            intState = intState % ((int)EnemyState.Last);
            SetState((EnemyState)intState);
        }

        private void SetState(EnemyState es)
        {
            state = es;
        }

        //public void OnPointerEnter(PointerEventData eventData)
        //{
        //    Debug.Log("ENtER");
        //}
    }
}
*/
}
