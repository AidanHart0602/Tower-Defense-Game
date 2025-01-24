using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private float _missileSpeed = 5;
    [SerializeField] private GameObject _particle;
    [SerializeField] private GameObject _explosionPrefab; 
    private AudioSource _audioSource; 

    // Use this for initialization
    void Start()
    {
        _audioSource = GetComponent<AudioSource>(); 
        _audioSource.pitch = Random.Range(0.7f, 1.9f);
        _audioSource.Play(); 
    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * _missileSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Ground"))
        {
            Instantiate(_explosionPrefab, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}