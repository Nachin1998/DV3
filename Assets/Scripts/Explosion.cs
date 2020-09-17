using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    ParticleSystem explosion;

    // Update is called once per frame
    private void Start()
    {
        explosion = GetComponent<ParticleSystem>();
    }
    void Update()
    {
        StartCoroutine(Vanish());
    }

    IEnumerator Vanish()
    {
        yield return new WaitForSeconds(2f);
        explosion.Stop();
        Destroy(gameObject, 2f);
    }
}
