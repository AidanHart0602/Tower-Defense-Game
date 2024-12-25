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
    [SerializeField]
    public float health = 100;
    private bool _damageCooldown = false;
    private bool _deathMoney = false;
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
    public void Injured(float DamageAmount)
    {

        if (_damageCooldown == false)
        {
            health = health - DamageAmount;
            if (health < 1)
            {
                _navMesh.isStopped = true;
                Destroy(this.gameObject);
            }

            if (_deathMoney == false)
            {
                _uimanager.money += 150;
                _deathMoney = true;
            }
         
            _damageCooldown = true;
            StartCoroutine(DamageCooldown());
        }
    }

    IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(.3f);
        _damageCooldown = false;
    }
}
