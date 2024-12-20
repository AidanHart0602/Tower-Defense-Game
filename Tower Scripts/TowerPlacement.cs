using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager;
    private TowerSpot _towerSpots, _selectedSpot;
    [SerializeField] private Camera _playerCam;
    [SerializeField] private GameObject _towerPlacementRadius;
    private GameObject _currentTower;
    private bool _canPlace;
    public LayerMask raycastLayers;
    private int _towerCost;

    void Start()
    {
        _towerSpots = FindFirstObjectByType<TowerSpot>();
    }

    void Update()
    {
        if(_currentTower != null)
        {
            Ray CameraRay = _playerCam.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(CameraRay, out RaycastHit info, 100f, raycastLayers))
            {
             
                _currentTower.transform.position = info.point;
                _towerPlacementRadius.transform.position = info.point;

                if (info.collider.CompareTag("Placement") && _uiManager.money >= _towerCost)
                {
                    _selectedSpot = info.collider.GetComponent<TowerSpot>();
                    _towerPlacementRadius.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                    _canPlace = true;
                }
                else
                {
                    _towerPlacementRadius.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                    _canPlace = false;
                }

                if (_canPlace == true && Input.GetMouseButtonDown(0))
                {
                    _uiManager.money -= _towerCost;
                    _selectedSpot.PlacedTurret();
                    _towerSpots.PlacementParticles(false);
                    _currentTower = null;
                    _towerPlacementRadius.SetActive(false);
                }
            }

            if (Input.GetMouseButton(1))
            {
                _towerSpots.PlacementParticles(false);
                Destroy(_currentTower);
                _towerPlacementRadius.SetActive(false);
            }
        }
    }
    public void PlaceTower(GameObject Tower)
    {
        Destroy(_currentTower);
        _towerSpots.PlacementParticles(true);
        _currentTower = Instantiate(Tower, Vector3.zero, Quaternion.identity);
        _towerPlacementRadius.SetActive(true);
    }

    public void TowerSelect(int ID) 
    {
        switch (ID) 
        {
            case 1:
                _towerCost = 200;
                break;
            case 2:
                _towerCost = 500;
                break;
        }
    }
}
