using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    [SerializeField]
    private GameObject _selectCollider, _spotDeactivater;
    [SerializeField]
    public int health = 100;
    private bool _destroyed = false; 

    public void Placed()
    {
        _selectCollider.SetActive(true);
        gameObject.transform.tag = "Turret";
        _spotDeactivater.SetActive(true);
    }

    public void Attacked(int Damage)
    {
        health -= Damage;
        if (health < 1 && _destroyed == false)
        {
            _destroyed = true;
            Destroy(gameObject);
        }
    }
}
