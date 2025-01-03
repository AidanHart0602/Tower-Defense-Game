using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseMissileLauncher;
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
                    _currentTower.GetComponent<TowerHP>().Placed();
                    _selectedSpot.PlacedTurret();
                    _towerSpots.PlacementParticles(false);
                    _currentTower = null;
                    _towerPlacementRadius.SetActive(false);
                }
            }

            if (Input.GetMouseButton(1))
            {
                Debug.Log("Destroyed Tower");
                _towerSpots.PlacementParticles(false);
                Destroy(_currentTower);
                _towerPlacementRadius.SetActive(false);
            }
        }
    }
    public void TowerType(GameObject Tower)
    {
        _currentTower = Instantiate(Tower, Vector3.zero, Quaternion.identity);
    }
    public void TowerID(int ID) 
    {
        Destroy(_currentTower);
        _towerSpots.PlacementParticles(true);
        switch (ID) 
        {
            case 1:
                _towerCost = 200;
                break;
            case 2:
                _towerCost = 500;
                break;
        }
        _towerPlacementRadius.SetActive(true);
    }
}
