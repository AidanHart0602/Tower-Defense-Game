using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyBehavior : MonoBehaviour
{
    private enum _aiState
    {
        Moving,
        Attacking,
        Dead
    }
    private Animator _anim;
    private _aiState _states;

    private NavMeshAgent _navMesh;
    [SerializeField] private GameObject[] _checkPoints;
    private int _checkPointCounter, _health = 100;

    void Start()
    {
        _navMesh = GetComponent<NavMeshAgent>();
        if (_navMesh != null)
        {
                _navMesh.destination = _checkPoints[_checkPointCounter].transform.position;
        }
        else
        {
            Debug.Log("Enemy NavMesh Agent is NULL!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_navMesh.remainingDistance < 1f)
        {
            _checkPointCounter++;
            _navMesh.destination = _checkPoints[_checkPointCounter].transform.position;
        }
    }
}
