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
    private _aiStates _state;

    private UIManager _uimanager;
    [SerializeField]
    private Animator _anim;
    private NavMeshAgent _navMesh;
    private GameObject _startingPoint, _finishPoint;
    [SerializeField]
    private Material _dissolver;
    [SerializeField]
    private Renderer _model;
    private TowerHP _currentTower;
    [SerializeField]
    public float health = 100;
    private bool _damageCooldown = false;
    private bool _deathMoney = false;
    private bool _dissolveActive = false;
    private bool _dissolveStrength;
    void Start()
    {
        _startingPoint = GameObject.FindGameObjectWithTag("Starting Point");
        _finishPoint = GameObject.FindGameObjectWithTag("Finish Point");
        _uimanager = FindObjectOfType<UIManager>();
        _navMesh = GetComponent<NavMeshAgent>();
        _state = _aiStates.running;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case _aiStates.running:
                _navMesh.destination = _finishPoint.transform.position;
                _navMesh.isStopped = false;
                _anim.SetBool("Attack", false);
                break;

            case _aiStates.attacking:
                _navMesh.isStopped = true;
                _anim.SetBool("Attack", true);
                break;

            case _aiStates.dead:
                _navMesh.isStopped = true;
                _model.material = _dissolver;
                _model.material.SetFloat("_Float", Mathf.MoveTowards(_model.material.GetFloat("_Float"), 1, Time.deltaTime));
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
            Debug.Log("Tower Detected");
            Quaternion TurretDirection = Quaternion.LookRotation((other.transform.position - transform.position).normalized);
            transform.rotation = TurretDirection;
          
            _currentTower.Attacked(10);
            if (_currentTower.health < 1)
            {
                _state = _aiStates.running;
                Destroy(other.gameObject);
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
        if (_damageCooldown == false)
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
            _damageCooldown = true;
            StartCoroutine(DamageCooldown());
        }
    }

    IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(.3f);
        _damageCooldown = false;
    }

    
    IEnumerator Death()
    {
        _state = _aiStates.dead;
        yield return new WaitForSeconds(1);
        this.gameObject.SetActive(false);
    }
}
