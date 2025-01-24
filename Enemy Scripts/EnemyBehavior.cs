using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyBehavior : MonoBehaviour
{
    private enum _aiStates
    {
        running,
        attacking,
        dead
    }

    private float _healthResetRef;
    [SerializeField] private _aiStates _state;
    private UIManager _uimanager;
    [SerializeField] private Animator _anim;
    [SerializeField] private Material _matRef;
    private NavMeshAgent _navMesh;
    private SpawnManager _spawnManager;
    private GameObject _finishPoint;
    [SerializeField]
    private Material _dissolver;
    [SerializeField]
    private Renderer _model;
    [SerializeField] private TowerHealth _currentTower;
    [SerializeField] public float health = 100;
    [SerializeField] private int _damage;
    [SerializeField] private float _speed;
    private bool _damageCD = false;
    private bool _death = false;
    private float _currentDissolveStrength;
    private float _addedDissolveStrength = 1;
    void Start()
    {
        _healthResetRef = health;
        _spawnManager = FindObjectOfType<SpawnManager>();
        _uimanager = FindObjectOfType<UIManager>();
        _navMesh = GetComponent<NavMeshAgent>();
        _finishPoint = GameObject.FindGameObjectWithTag("Finish Point");

        _state = _aiStates.running;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case _aiStates.running:
                if (_currentTower != null)
                {
                    _state = _aiStates.attacking;
                    return;
                }

                gameObject.tag = "Enemy";
                _navMesh.isStopped = false;
                _anim.SetBool("Attack", false);
                _navMesh.destination = _finishPoint.transform.position;
                _navMesh.speed = _speed;

                break;

            case _aiStates.attacking:
                if (_currentTower == null)
                {
                    _state = _aiStates.running;
                    return;
                }
                _navMesh.isStopped = true;
                _anim.SetBool("Attack", true);
                break;

            case _aiStates.dead:
                gameObject.tag = "Ignore";
                _anim.SetBool("Attack", false);
                _navMesh.isStopped = true;
                _model.material = _dissolver;
                _model.material.SetFloat("_DissolveStrength", _currentDissolveStrength);
                _currentDissolveStrength += _addedDissolveStrength * Time.deltaTime;
                break;
        }

        if (health < 1)
        {

            if (_death == false)
            {
                _death = true;
                StartCoroutine(Death());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Turret"))
        {
            _currentTower = other.GetComponent<TowerHealth>();
            _state = _aiStates.attacking;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Turret"))
        {
            Quaternion TurretDirection = Quaternion.LookRotation((other.transform.position - transform.position).normalized);
            transform.rotation = TurretDirection;
            if (_damageCD == false)
            {
                if (_state != _aiStates.attacking)
                {
                    _state = _aiStates.attacking;
                }

                if (_currentTower != null)
                {
                    _damageCD = true;
                    StartCoroutine(DamageCD(10));
                }
                else
                {
                    _damageCD = false;
                }

            }
        }

        if (other.CompareTag("Finish Point"))
        {
            _navMesh.isStopped = false;
            _uimanager.DecreaseUiHP();
            Reset();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Turret"))
        {
            _state = _aiStates.running;
        }
    }
    public void Injured(float DamageAmount)
    {
        health = health - DamageAmount;
    }

    private void Reset()
    {
        health = _healthResetRef;
        _death = false;
        _state = _aiStates.running;
        _model.material = _matRef;
        _currentTower = null;
        this.gameObject.SetActive(false);
    }

    IEnumerator DamageCD(int Damage)
    {
        yield return new WaitForSeconds(1.15f);
        _currentTower.Attacked(Damage);
        yield return new WaitForSeconds(1.5f);
        _damageCD = false;
    }

    IEnumerator Death()
    {
        _spawnManager.spawnedEnemies = _spawnManager.spawnedEnemies + 1;
        _uimanager.money += 150;
        _state = _aiStates.dead;
        _currentDissolveStrength = 0;
        yield return new WaitForSeconds(3);
        Reset();
    }
}