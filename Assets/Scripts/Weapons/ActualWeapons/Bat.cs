using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    public float damage;
    Animator animator;
    bool isHitting = false;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(!isHitting)
            {
                StartCoroutine(Hit());                      
            }
        }
    }

    public IEnumerator Hit()
    {
        isHitting = true;
        animator.SetBool("Hitting", true);
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("Hitting", false);
        isHitting = false;        
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Enemy"))
        {
            Debug.Log("Hit");
            col.GetComponent<BaseEnemy>().TakeDamage(damage);
            col.transform.position += -col.transform.forward * damage * Time.deltaTime;
        }       
    }
}
