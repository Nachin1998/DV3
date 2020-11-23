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
                AkSoundEngine.PostEvent("player_batair", gameObject);
                StartCoroutine(Hit());
            }
        }
    }

    public IEnumerator Hit()
    {
        isHitting = true;
        animator.SetBool("Hitting", true);
        collider.isTrigger = true;

        yield return new WaitForSeconds(0.3f);

        collider.isTrigger = false;
        animator.SetBool("Hitting", false);
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

    private void OnEnable()
    {
        transform.position = new Vector3(0, -1, 0);
        isHitting = false;
    }

    private void OnDisable()
    {
        transform.position = new Vector3(0, -1, 0);
        isHitting = false;
    }
}
