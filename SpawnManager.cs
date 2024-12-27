using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _enemyPool;
    [SerializeField]
    private GameObject _enemyType;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _startingPoint;
    private int _enemyLimit = 325;
    [SerializeField]
    private int _spawnedEnemies = 0;
    [SerializeField]
    private int _numberOfEnemies;
    private bool _gameActive = true;
    List<GameObject> _storeEnemies(int NumOfEnems)
    {
        for(int i = 0; i < NumOfEnems; i++) 
        {
            GameObject enemy = Instantiate(_enemyType);
            enemy.transform.position = _startingPoint.transform.position;
            _enemyPool.Add(enemy);
        }
        return _enemyPool;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

//    IEnumerator StoreAI
}
