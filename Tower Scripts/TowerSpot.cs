using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpot : MonoBehaviour
{
    private static bool _particlesActive;
    private bool _placed = false;
    [SerializeField]
    private GameObject _particles;
 

    // Update is called once per frame
    void Update()
    {
        if(_particlesActive == true && _placed == false) 
        {
            _particles.SetActive(true);
        }

        else
        {
            _particles.SetActive(false);
        }
    }
    public void PlacementParticles(bool Active)
    {
        _particlesActive = Active;
    }

    public void PlacedTurret()
    {
        _placed = true;
    }

    public void RemovedTurret()
    {
        _placed = false;
        gameObject.layer = LayerMask.NameToLayer("PlaceableTurret");
    }

    private void OnTriggerStay(Collider other)
    {
        if (_placed == true)
        {
            gameObject.layer = LayerMask.NameToLayer("Ignore");
        }
    }
}
