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
        ChasePlayer();
    }

    public override void ChasePlayer()
    {
        if (isDead || playerTarget.isDead)
        {
            return;
        }

        if (playerTarget)
        {
            Vector3 direction = playerTarget.transform.position - transform.position;
            if (direction.magnitude > 40)
            {
                agent.stoppingDistance = 40;
                agent.SetDestination(playerTarget.transform.position);
            }
            else if (direction.magnitude < 20)
            {
                agent.stoppingDistance = 0;
                agent.speed = 40;
                agent.SetDestination(agent.transform.position - direction * Time.deltaTime * agent.speed);
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
    }

    void Fire()
    {
        Instantiate(projectile, fireSpot.position, fireSpot.rotation);
        attackSpeedRate = maxAttackSpeedRate;
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
