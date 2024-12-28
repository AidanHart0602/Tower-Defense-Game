using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private UIManager _uiManager;
    private List<GameObject> _enemyPool;
    [SerializeField]
    private GameObject[] _enemyType;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _startingPoint;

    private int _waveNum, _waveEnemyNum, _spawnedEnemies;
    private int _waveEnemyLimit = 5;
    List<GameObject> _storeEnemies(int NumOfEnems)
    {
        for (int i = 0; i < NumOfEnems; i++)
        {
            GameObject enemy = Instantiate(_enemyType[0]);
            enemy.transform.position = _startingPoint.transform.position;
            enemy.SetActive(false);
            _enemyPool.Add(enemy);
        }
        return _enemyPool;
    }

    void Start()
    {
        _storeEnemies(10);
        NewWave();
    }
    private void NewWave() 
    {
        _waveNum++;
        _uiManager.waveUpdate(_waveNum);
        if (_waveNum == 11)
        {
            //Start the end sequence
            return;
        }
        _waveEnemyLimit += 5;
        _waveEnemyNum = _waveEnemyLimit;
        _uiManager.enemyNumChange(_waveEnemyNum);
        _spawnedEnemies = 0;
        StartCoroutine(SpawnAI());
    }

    public void ReduceNumbers() 
    {
        _waveEnemyNum--;
        _uiManager.enemyNumChange(_waveEnemyNum);
    }
    IEnumerator SpawnAI()
    {
        while((_spawnedEnemies > _waveEnemyLimit ) && (_spawnedEnemies != _waveEnemyLimit))
        {
            foreach(var enemy in _enemyPool) 
            {
                if(enemy.activeInHierarchy == false)
                {
                    _spawnedEnemies++;
                    enemy.SetActive(true);
                    break;
                }
            }
            yield return new WaitForSeconds(0.5f);
        }

        if((_spawnedEnemies == _waveEnemyLimit) && _waveNum != 11)
        {
            NewWave();
        }
    }
}
