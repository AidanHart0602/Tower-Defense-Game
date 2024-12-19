using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    [SerializeField]
    private Camera _playerCam;
    
    private GameObject _currentTower;

    private bool _canPlace;
    public LayerMask raycastLayers;

    void Start()
    {
    }

    void Update()
    {
        if(_currentTower != null)
        {
            Ray CameraRay = _playerCam.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(CameraRay, out RaycastHit info, 100f, raycastLayers))
            {
             
                _currentTower.transform.position = info.point;
              
                if (info.collider.CompareTag("Placement"))
                {
                    _canPlace = true;
                }
                else
                {
                    _canPlace = false;
                }
            }

            if (_canPlace == true && Input.GetMouseButtonDown(0))
            {
                _currentTower = null;
            }

            if (Input.GetMouseButton(1))
            {
                Destroy(_currentTower);
            }
        }
    }
    public void PlaceTower(GameObject Tower)
    {
        Destroy(_currentTower);
        _currentTower = Instantiate(Tower, Vector3.zero, Quaternion.identity);
    }
}
