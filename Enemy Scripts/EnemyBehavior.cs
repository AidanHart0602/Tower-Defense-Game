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
    private float _speedHealthRef; 
    private static Animator _globalAnim;
    public static NavMeshAgent _globalNavMesh;

    private _aiStates _state;
    private UIManager _uimanager;
    [SerializeField] private Animator _anim;
    [SerializeField] private Material _matRef;
    private NavMeshAgent _navMesh;

    private GameObject _startingPoint, _finishPoint;
    [SerializeField]
    private Material _dissolver;
    [SerializeField]
    private Renderer _model;
    private SpawnManager _spawnManager;
    private TowerHP _currentTower;
    [SerializeField] public float health = 100;

    [SerializeField] private int _damage;
    private bool _damageCD = false;
    private bool _deathMoney = false;
    void Start()
    {

        _globalAnim = _anim;
        _healthResetRef = health;
        _globalNavMesh = GetComponent<NavMeshAgent>();
        
        _spawnManager = FindObjectOfType<SpawnManager>();
        _uimanager = FindObjectOfType<UIManager>();
        _navMesh = GetComponent<NavMeshAgent>();

        _startingPoint = GameObject.FindGameObjectWithTag("Starting Point");
        _finishPoint = GameObject.FindGameObjectWithTag("Finish Point");
        
        
        _state = _aiStates.running;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case _aiStates.running:
                _anim.SetBool("Attack", false);
                _navMesh.destination = _finishPoint.transform.position;
                Vector3 destination = _navMesh.destination;
                if(transform.position == destination)
                {
                    _navMesh.isStopped = false;
                    Reset();
                }
                break;

            case _aiStates.attacking:
                _navMesh.isStopped = true;
                _anim.SetBool("Attack", true);
                break;

            case _aiStates.dead:
                gameObject.tag = "Ignore";
                _anim.SetBool("attack", false);
                _navMesh.isStopped = true;
                _model.material = _dissolver;
                _model.material.SetFloat("_DissolveStrength", Mathf.Sin(Time.time));
                break;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Turret"))
        {
            _currentTower = other.GetComponent<TowerHP>();
            _state = _aiStates.attacking;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Turret"))
        {
            Quaternion TurretDirection = Quaternion.LookRotation((other.transform.position - transform.position).normalized);
            transform.rotation = TurretDirection;
            if(_damageCD == false)
            {
                _damageCD = true;
                StartCoroutine(DamageCD(10));
            }
            
            if (_currentTower.health < 1)
            {
                _state = _aiStates.running;
            }   
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
            if (health < 1)
            {
                if (_deathMoney == false)
                {
                    _uimanager.money += 150;
                    _deathMoney = true;
                }
                StartCoroutine(Death());
            }
    }

    public void Pause()
    {
        _globalNavMesh.isStopped = true;
        _globalAnim.speed = 0;
    }

    public void RegularSpeed()
    {
        _globalNavMesh.isStopped = false;
        _globalAnim.speed = 1;
        _globalNavMesh.speed = 1;
    }

    public void SpeedUp()
    {
        health = _healthResetRef;
        _globalNavMesh.speed = 2;
        _globalAnim.speed = 2;
    }

    private void Reset()
    {
        health = _healthResetRef;
        _state = _aiStates.running;
        _model.material = _matRef;
        gameObject.tag = "Enemy";
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
        _state = _aiStates.dead;
        yield return new WaitForSeconds(3);
        Reset();
       
    }
}
