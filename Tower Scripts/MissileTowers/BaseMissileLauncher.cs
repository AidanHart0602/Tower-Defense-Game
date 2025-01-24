using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMissileLauncher : MonoBehaviour
{


    [SerializeField] private List<GameObject> _enemies;
    [SerializeField] private GameObject _mainTarget;
    [SerializeField] private GameObject _parent;
    private bool _attackCD = false;

    [SerializeField]
    private GameObject _missilePrefab;
    [SerializeField] private GameObject[] _misslePositions; 
    private float _fireDelay = 3; 
    [SerializeField] private float _reloadTime = 2; 


    private void Update()
    {
        if (_mainTarget != null && _parent.tag == "Turret")
        {
            Quaternion TargetLookAt = Quaternion.LookRotation((_mainTarget.transform.position - transform.position).normalized);
            transform.rotation = TargetLookAt;
        }
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

    IEnumerator FireRocketsRoutine()
    {
        yield return new WaitForSeconds(5);
        if(_mainTarget != null)
        {
            for (int i = 0; i < _misslePositions.Length; i++)
            {
                if(_mainTarget != null)
                {
                    GameObject rocket = Instantiate(_missilePrefab);
                    rocket.transform.parent = _misslePositions[i].transform;
                    rocket.transform.localPosition = Vector3.zero;
                    rocket.transform.localEulerAngles = new Vector3(-90, 0, 0);
                    rocket.transform.parent = null;
                    _misslePositions[i].SetActive(false);
                    yield return new WaitForSeconds(_fireDelay);
                }
            }
        }
        for (int i = 0; i < _misslePositions.Length; i++)
        {
            yield return new WaitForSeconds(_reloadTime);
            _misslePositions[i].SetActive(true);
        }
        _attackCD = false;
    }
}