using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualMissileLauncher : MonoBehaviour
{

    [SerializeField] private List<GameObject> _enemies;
    [SerializeField] private GameObject _mainTarget;
    [SerializeField] private GameObject _parent;
    private bool _attackCD = false;
    [SerializeField] private GameObject TurretOne, TurretTwo;
    [SerializeField] private GameObject _missilePrefab; //holds the missle gameobject to clone
    [SerializeField] private GameObject[] _misslePositionsLeft; //array to hold the rocket positions on the turret
    [SerializeField] private GameObject[] _misslePositionsRight; //array to hold the rocket positions on the turret
    private float _fireDelay = 1.5f; //fire delay between rockets
    private float _reloadTime = 2; //time in between reloading the rockets

    private void Update()
    {
        if (_mainTarget != null && _parent.tag == "Turret") //Essentially checks if the turret has been placed before finding a target
        {
            Quaternion TargetLookAt = Quaternion.LookRotation((_mainTarget.transform.position - transform.position).normalized);
            transform.rotation = TargetLookAt;
            if (TurretOne && TurretTwo != null)
            {
                Quaternion LauncherLookAtOne = Quaternion.LookRotation(_mainTarget.transform.position - TurretOne.transform.position);
                Quaternion LauncherLookAtTwo = Quaternion.LookRotation(_mainTarget.transform.position - TurretTwo.transform.position);
                TurretOne.transform.rotation = LauncherLookAtOne;
                TurretTwo.transform.rotation = LauncherLookAtTwo;
            }

        }
    }

    IEnumerator FireRocketsRoutine()
    {
        yield return new WaitForSeconds(5);
        for (int i = 0; i < _misslePositionsLeft.Length; i++) //for loop to iterate through each missle position
        {
            if (_mainTarget != null)
            {
                GameObject rocketLeft = Instantiate(_missilePrefab) as GameObject; //instantiate a rocket
                GameObject rocketRight = Instantiate(_missilePrefab) as GameObject; //instantiate a rocket

                rocketLeft.transform.parent = _misslePositionsLeft[i].transform; //set the rockets parent to the missle launch position 
                rocketRight.transform.parent = _misslePositionsRight[i].transform; //set the rockets parent to the missle launch position 

                rocketLeft.transform.localPosition = Vector3.zero; //set the rocket position values to zero
                rocketRight.transform.localPosition = Vector3.zero; //set the rocket position values to zero

                rocketLeft.transform.localEulerAngles = new Vector3(0, 0, 0); //set the rotation values to be properly aligned with the rockets forward direction
                rocketRight.transform.localEulerAngles = new Vector3(0, 0, 0); //set the rotation values to be properly aligned with the rockets forward direction

                rocketLeft.transform.parent = null; //set the rocket parent to null
                rocketRight.transform.parent = null; //set the rocket parent to null

                _misslePositionsLeft[i].SetActive(false); //turn off the rocket sitting in the turret to make it look like it fired
                _misslePositionsRight[i].SetActive(false); //turn off the rocket sitting in the turret to make it look like it fired

            }
            yield return new WaitForSeconds(_fireDelay); //wait for the firedelay

        }


        for (int i = 0; i < _misslePositionsLeft.Length; i++) //itterate through missile positions
        {
            yield return new WaitForSeconds(_reloadTime); //wait for reload time
            _misslePositionsLeft[i].SetActive(true); //enable fake rocket to show ready to fire
            _misslePositionsRight[i].SetActive(true); //enable fake rocket to show ready to fire
        }
        _attackCD = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _enemies.Add(other.gameObject);
            TargetCheck();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _enemies.Remove(other.gameObject);
        _mainTarget = null;
        TargetCheck();
    }

    private void OnTriggerStay(Collider other)
    {
        if (_mainTarget == null)
        {
            TargetCheck();
        }
        if (_parent.tag == "Turret" && _attackCD == false)
        {
            if (other.CompareTag("Enemy"))
            {
                StartCoroutine(FireRocketsRoutine());
                _attackCD = true;
            }
        }
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
}