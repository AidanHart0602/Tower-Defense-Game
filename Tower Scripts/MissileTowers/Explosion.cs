using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private Collider _collider;
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider>();
        StartCoroutine(DestroyExplosion());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyBehavior>().Injured(40);
        }
        else
        {
            return;
        }
    }
    IEnumerator DestroyExplosion()
    {
        yield return new WaitForSeconds(1.0f);
        _collider.enabled = false;
        yield return new WaitForSeconds(3.9f);
        Destroy(gameObject);
    }
}
