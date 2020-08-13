using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    ParticleSystem explosion;
    float maxTimer = 5f;
    float timer = 0;

    // Update is called once per frame
    private void Start()
    {
        explosion = GetComponent<ParticleSystem>();
    }
    void Update()
    {
        timer += Time.deltaTime;

        StartCoroutine(Vanish());
    }

    IEnumerator Vanish()
    {
        yield return new WaitForSeconds(2);
        explosion.Stop();
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
