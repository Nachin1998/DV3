using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5;
    public float lifeTime = 5;
    Player target;
    Vector3 direction;
    void Start()
    {
        target = FindObjectOfType<Player>();
        direction = (target.transform.position - transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;

        transform.position += direction * speed * Time.deltaTime;

        if(lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        switch (col.tag)
        {
            case "Player":
                col.GetComponent<Player>().TakeDamage(10f);
                Destroy(gameObject);
                break;

            case "Ground":
            case "Rock":
            case "Tree":
                Destroy(gameObject);
                break;
        }
    }
}
