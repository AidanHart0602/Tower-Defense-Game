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
    private UIManager _uimanager;
    private Animator _anim;
    private _aiState _states;

    private NavMeshAgent _navMesh;
    private GameObject _startingPoint, _finishPoint;
    private int _checkPointCounter, _health = 100;

    void Start()
    {
        _startingPoint = GameObject.FindGameObjectWithTag("Starting Point");
        _finishPoint = GameObject.FindGameObjectWithTag("Finish Point");
        _uimanager = FindObjectOfType<UIManager>();
        _navMesh = GetComponent<NavMeshAgent>();
        if (_navMesh != null)
        {
            _navMesh.destination = _finishPoint.transform.position;
        }
        else
        {
            Debug.Log("Enemy NavMesh Agent is NULL!");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Damage(int DamageAmount)
    {
        _health -= DamageAmount;
        if(_health < 0)
        {
            _uimanager.money += 150;
        }
    }
}
