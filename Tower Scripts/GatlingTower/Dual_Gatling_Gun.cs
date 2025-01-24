using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This script will allow you to view the presentation of the Turret and use it within your project.
/// Please feel free to extend this script however you'd like. To access this script from another script
/// (Script Communication using GetComponent) -- You must include the namespace (using statements) at the top. 
/// "using GameDevHQ.FileBase.Dual_Gatling_Gun" without the quotes. 
/// 
/// For more, visit GameDevHQ.com
/// 
/// @authors
/// Al Heck
/// Jonathan Weinberger
/// </summary>

[RequireComponent(typeof(AudioSource))] //Require Audio Source component
public class DualGatlingGun : MonoBehaviour
{
    [SerializeField] private List<GameObject> _enemies;
    [SerializeField] private GameObject _mainTarget;

    private EnemyBehavior _enemyAI;
    [SerializeField] private GameObject _parent;
    private bool _attackCD = false;
    [SerializeField] private Transform[] _gunBarrel; //Reference to hold the gun barrel
    [SerializeField] private GameObject[] _muzzleFlash; //reference to the muzzle flash effect to play when firing
    [SerializeField]
    private ParticleSystem[] _bulletCasings; //reference to the bullet casing effect to play when firing
    [SerializeField] private AudioClip _fireSound; //Reference to the audio clip

    private AudioSource _audioSource; //reference to the audio source component
    private bool _startWeaponNoise = true;

    // Use this for initialization
    void Start()
    {
        _muzzleFlash[0].SetActive(false); //setting the initial state of the muzzle flash effect to off
        _muzzleFlash[1].SetActive(false); //setting the initial state of the muzzle flash effect to off
        _audioSource = GetComponent<AudioSource>(); //ssign the Audio Source to the reference variable
        _audioSource.playOnAwake = false; //disabling play on awake
        _audioSource.loop = true; //making sure our sound effect loops
        _audioSource.clip = _fireSound; //assign the clip to play
    }

    // Update is called once per frame
    void Update()
    {
        if (_mainTarget != null && _parent.tag == "Turret") //Essentially checks if the turret has been placed before finding a target
        {
            _enemyAI = _mainTarget.GetComponent<EnemyBehavior>();
            Quaternion TargetLookAt = Quaternion.LookRotation((_mainTarget.transform.position - transform.position).normalized);
            transform.rotation = TargetLookAt;

            if (_attackCD == false)
            {
                _enemyAI.Injured(10);
                _attackCD = true;
                StartCoroutine(AttackCooldown(.1f));
            }

            if (_enemyAI.health < 1)
            {
                _enemies.Remove(_mainTarget);
                _mainTarget = null;
                StopFiring();
                TargetCheck();
            }
        }
    }
    void RotateBarrel()
    {
        _gunBarrel[0].transform.Rotate(Vector3.forward * Time.deltaTime * -500.0f); //rotate the gun barrel along the "forward" (z) axis at 500 meters per second
        _gunBarrel[1].transform.Rotate(Vector3.forward * Time.deltaTime * -500.0f); //rotate the gun barrel along the "forward" (z) axis at 500 meters per second
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _enemies.Add(other.gameObject);
            TargetCheck();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_mainTarget == null)
        {
            StopFiring();
            TargetCheck();
        }
        if (_parent.tag == "Turret")
        {
            if (other.CompareTag("Enemy"))
            {
                StartFiring();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _enemies.Remove(other.gameObject);
            _mainTarget = null;
            StopFiring();
            TargetCheck();
        }
    }

    void StartFiring()
    {
        RotateBarrel(); //Call the rotation function responsible for rotating our gun barrel

        //for loop to iterate through all muzzle flash objects
        for (int i = 0; i < _muzzleFlash.Length; i++)
        {
            _muzzleFlash[i].SetActive(true); //enable muzzle effect particle effect
            _bulletCasings[i].Emit(1); //Emit the bullet casing particle effect   
        }

        if (_startWeaponNoise == true) //checking if we need to start the gun sound
        {
            _audioSource.Play(); //play audio clip attached to audio source
            _startWeaponNoise = false; //set the start weapon noise value to false to prevent calling it again
        }
    }

    void StopFiring()
    {
        //for loop to iterate through all muzzle flash objects
        for (int i = 0; i < _muzzleFlash.Length; i++)
        {
            _muzzleFlash[i].SetActive(false); //Disable muzzle effect particle effect
        }
        _audioSource.Stop(); //stop the sound effect from playing
        _startWeaponNoise = true; //set the start weapon noise value to true
    }

    private void TargetCheck()
    {
        if (_mainTarget != null)
        {
            return;
        }

        GameObject NearestEnemy = null;
        float ClosestDistance = float.MaxValue;

        foreach (GameObject enemy in _enemies)
        {
            float CurrentEnemyDistance = Vector3.Distance(transform.position, enemy.transform.position);

            if (enemy == null)
            {
                return;
            }

            if (CurrentEnemyDistance < ClosestDistance)
            {
                ClosestDistance = CurrentEnemyDistance;
                NearestEnemy = enemy;
            }
        }

        if (NearestEnemy != null)
        {
            _mainTarget = NearestEnemy;
        }
        else
        {
            _mainTarget = null;
        }
    }

    IEnumerator AttackCooldown(float CD)
    {
        yield return new WaitForSeconds(CD);
        _attackCD = false;
    }
}