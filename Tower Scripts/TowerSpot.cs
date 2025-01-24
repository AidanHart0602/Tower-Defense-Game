using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpot : MonoBehaviour
{
    private static bool _particlesActive;
    [SerializeField] private bool _placed = false;
    [SerializeField] private GameObject _particles;
    public GameObject currentTurret;

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

        if(currentTurret == null)
        {
            currentTurret = null;
            _placed = false;
            gameObject.layer = LayerMask.NameToLayer("PlaceableTurret");
        }
    }
    public void PlacementParticles(bool Active)
    {
        _particlesActive = Active;
    }



    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("DeactivateSpot"))
        {
            other.gameObject.transform.parent.position = transform.position;
            if (_placed == false)
            {
                _placed = true;
      
                currentTurret = other.gameObject;
                gameObject.layer = LayerMask.NameToLayer("Ignore");
            }
            return;
        }
    }
}
