using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    public float damage;
    Animator animator;
    bool isHitting = false;
    Collider collider;
    void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();
    }

    void Update()
    {
        if (Pause.gameIsPaused)
        {
            return;
        }

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
        animator.SetBool("isAttacking", true);

        yield return new WaitForSeconds(0.3f);

        collider.isTrigger = true;
        AkSoundEngine.PostEvent("player_batair", gameObject);
        yield return new WaitForSeconds(0.1f);
        collider.isTrigger = false;

        yield return new WaitForSeconds(0.9f);

        animator.SetBool("isAttacking", false);
        isHitting = false;        
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Enemy"))
        {
            AkSoundEngine.PostEvent("player_bathit", gameObject);
            col.GetComponent<BaseEnemy>().TakeDamage(damage);
            col.transform.position += -col.transform.forward * damage * Time.deltaTime;
        }       
    }
}
