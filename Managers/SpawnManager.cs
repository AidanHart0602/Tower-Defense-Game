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
    [SerializeField] private int _firstEnemyLimit, _secondEnemyLimit, _totalSpawnedLimit;
    [SerializeField] public int spawnedEnemies;

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
        _waveNum = 1;
        _uiManager.waveUpdate(_waveNum);
        StoreFirstEnemyType(55);
        StoreSecondEnemyType(27);
        NewWave();
    }
    private void NewWave()
    {
        spawnedEnemies = 0;
        if (_waveNum == 11)
        {
            return;
        }


        if (_waveNum < 2)
        {
            _NumOfEnemies = new Vector2Int(10, 0);
            _spawnComplete = false;
            _firstEnemyLimit = _NumOfEnemies.x;
            _secondEnemyLimit = _NumOfEnemies.y;
            _totalSpawnedLimit = _firstEnemyLimit + _secondEnemyLimit;
            _numOfFirstEnems = 0;
            _numOfSecondEnems = 0;
        }

        if (_waveNum >= 2)
        {
            _NumOfEnemies = new Vector2Int(_NumOfEnemies.x + 5, _NumOfEnemies.y + 3);
            _spawnComplete = false;
            _firstEnemyLimit = _NumOfEnemies.x;
            _secondEnemyLimit = _NumOfEnemies.y;
            _totalSpawnedLimit = _firstEnemyLimit + _secondEnemyLimit;
            _numOfFirstEnems = 0;
            _numOfSecondEnems = 0;
        }

        StartCoroutine(SpawnAINumOne());
        StartCoroutine(SpawnAINumTwo());
    }
    IEnumerator SpawnAINumTwo()
    {
        while (_spawnComplete != true)
        {
            foreach (GameObject enemy in _enemyTypeTwoPool)
            {
                if ((_waveNum >= 2 && (_secondEnemyLimit > _numOfSecondEnems)))
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
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator SpawnAINumOne()
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

            if ((spawnedEnemies == _totalSpawnedLimit))
            {
                _spawnComplete = true;
            }
            yield return new WaitForSeconds(0.01f);
        }

        if (_spawnComplete == true && _waveNum != 11)
        {
            _waveNum++;
            _uiManager.waveUpdate(_waveNum);
            yield return new WaitForSeconds(5.0f);
            NewWave();
        }
    }
}