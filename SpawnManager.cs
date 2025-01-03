using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private GameObject[] _enemyType;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject _startingPoint;
    [SerializeField] private List<GameObject> _enemyTypeOnePool, _enemyTypeTwoPool;
    private Vector2Int _NumOfEnemies;
    [SerializeField] private int _waveNum, _numOfFirstEnems, _numOfSecondEnems;
    private bool _spawnComplete;
    [SerializeField] private int _firstEnemyLimit, _secondEnemyLimit;

    List<GameObject> StoreSecondEnemyType(int NumOfEnems)
    {
        for (int i = 0; i < NumOfEnems; i++)
        {
            GameObject enemy = Instantiate(_enemyType[1]);
            enemy.transform.parent = _enemyContainer.transform;
            enemy.SetActive(false);
            _enemyTypeTwoPool.Add(enemy);
        }
        return _enemyTypeTwoPool;
    }

    List<GameObject> StoreFirstEnemyType(int NumOfEnems)
    {
        for (int i = 0; i < NumOfEnems; i++)
        {
            GameObject enemy = Instantiate(_enemyType[0]);
            enemy.transform.parent = _enemyContainer.transform;
            enemy.SetActive(false);
            _enemyTypeOnePool.Add(enemy);
        }
        return _enemyTypeOnePool;
    }

    void Start()
    {
        StoreFirstEnemyType(55);
        StoreSecondEnemyType(27);
        NewWave();
    }
    private void NewWave()
    {
        _waveNum++;
        if (_waveNum == 11)
        {
            //Start the end sequence
            return;
        }
        _uiManager.waveUpdate(_waveNum);

        if (_waveNum < 2)
        {
            _NumOfEnemies = new Vector2Int(10, 0);
            _spawnComplete = false;
            _firstEnemyLimit = _NumOfEnemies.x;
            _secondEnemyLimit = _NumOfEnemies.y;
            _numOfFirstEnems = 0;
            _numOfSecondEnems = 0;
        }

        if (_waveNum >= 2)
        {
            _NumOfEnemies = new Vector2Int(_NumOfEnemies.x + 5, _NumOfEnemies.y + 3);
            _spawnComplete = false;
            _firstEnemyLimit = _NumOfEnemies.x;
            _secondEnemyLimit = _NumOfEnemies.y;
            _numOfFirstEnems = 0;
            _numOfSecondEnems = 0;
        }

        StartCoroutine(SpawnAI());
    }

    public void ReduceNumbers()
    {

    }
    IEnumerator SpawnAI()
    {
        while (_spawnComplete != true)
        {
            foreach (GameObject enemy in _enemyTypeOnePool)
            {
                if (_firstEnemyLimit > _numOfFirstEnems)
                {
                    if (enemy.activeInHierarchy == false)
                    {
                        enemy.transform.position = _startingPoint.transform.position;
                        _numOfFirstEnems++;
                        enemy.SetActive(true);
                        yield return new WaitForSeconds(2f);
                    }
                }
            }

            foreach (GameObject enemy in _enemyTypeTwoPool)
            {
                if (_waveNum >= 2 && (_secondEnemyLimit > _numOfSecondEnems))
                {
                    if (enemy.activeInHierarchy == false)
                    {
                        enemy.transform.position = _startingPoint.transform.position;
                        _numOfSecondEnems++;
                        enemy.SetActive(true);
                        yield return new WaitForSeconds(6f);
                    }
                }
            }

            if ((_numOfFirstEnems == _firstEnemyLimit) && (_numOfSecondEnems == _secondEnemyLimit))
            {
                _spawnComplete = true;
            }
        }

        if (_spawnComplete == true && _waveNum != 11)
        {
            yield return new WaitForSeconds(40);
            NewWave();
        }
    }
}