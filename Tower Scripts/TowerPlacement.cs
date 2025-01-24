using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{

    [SerializeField] private UIManager _uiManager;
    private TowerSpot _towerSpots, _selectedSpot;
    [SerializeField] private Camera _playerCam;
    [SerializeField] private GameObject _towerPlacementRadius;
    [SerializeField] private GameObject _currentTower;
    private bool _canPlace, _canSelect;
    public LayerMask placementLayers;
    private int _towerCost;
    [SerializeField] private GameObject _towerSelect;
    public LayerMask selectTowerLayer;
    void Start()
    {
        _towerSpots = FindFirstObjectByType<TowerSpot>();
    }

    void Update()
    {
        if(_currentTower != null)
        {
            Ray CameraRay = _playerCam.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(CameraRay, out RaycastHit info, 100f, placementLayers))
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
                    _currentTower.GetComponentInChildren<TowerHealth>().Placed();
                  //  _selectedSpot.PlacedTurret(_currentTower);
                    _towerSpots.PlacementParticles(false);
                    _currentTower = null;
                    _towerPlacementRadius.SetActive(false);
                    StartCoroutine(Select());
                }
            }

            if (Input.GetMouseButton(1))
            {
                _towerSpots.PlacementParticles(false);
                Destroy(_currentTower);
                _towerPlacementRadius.SetActive(false);
            }
        }

        if(_currentTower == null)
        {
            Ray CameraRay = _playerCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(CameraRay, out RaycastHit info, 100f, selectTowerLayer))
            {
                if (info.collider.CompareTag("TowerSelect") && Input.GetMouseButtonDown(0) && _canSelect == true)
                {
                    _towerSelect = info.collider.gameObject;

                    if (_towerSelect.transform.parent.GetComponent<LauncherTowerUpgrade>() != null)
                    {
                        _uiManager.UpgradeMissilePopUp(true);
                    }

                    else if(_towerSelect.transform.parent.GetComponent<GatlingTowerUpgrade>() != null)
                    {
                        _uiManager.UpgradeGatlingPopUp(true);
                    }
                }
            }
        }
    }

    public void TowerType(GameObject Tower)
    {
        _currentTower = Instantiate(Tower, Vector3.zero, Quaternion.identity);
    }

    public void UpgradeTower()
    {
        if (_towerSelect.GetComponentInParent<GatlingTowerUpgrade>() == null && _uiManager.money >= 750)
        {
            _towerSelect.GetComponentInParent<LauncherTowerUpgrade>().UpgradeTurret();
            _uiManager.money -= 750;
            _towerSelect = null;
            _uiManager.UpgradeMissilePopUp(false);
        }
        else if(_towerSelect.GetComponentInParent<LauncherTowerUpgrade>() == null && _uiManager.money >= 500)
        {
            _towerSelect.GetComponentInParent<GatlingTowerUpgrade>().UpgradeTurret();
            _uiManager.money -= 500;
            _towerSelect = null;
            _uiManager.UpgradeGatlingPopUp(false);
        }
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

    IEnumerator Select()
    {
        _canSelect = false;
        yield return new WaitForSeconds(.3f);
        _canSelect = true;
    }
}
