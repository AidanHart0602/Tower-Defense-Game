using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHP : MonoBehaviour
{
    [SerializeField]
    private GameObject _selectCollider;
    [SerializeField]
    public int health = 100;
    private TowerSpot _spot;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Placement"))
        {
            _spot = other.GetComponent<TowerSpot>();
        }
        if (health < 1) 
        {
            _spot.GetComponent<TowerSpot>().RemovedTurret();
            Destroy(this.gameObject);
        }
    }
    public void Placed()
    {
        _selectCollider.SetActive(true);
        gameObject.tag = "Turret";
    }

    public void Attacked(int Damage)
    {
        health -= Damage;
        if (health < 1)
        {
            Destroy(gameObject);
            _spot.RemovedTurret();
        }
    }
}
