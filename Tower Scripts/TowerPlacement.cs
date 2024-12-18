using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    [SerializeField]
    private Camera _playerCam;

    private GameObject _currentTower;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if(_currentTower != null)
        {
            Ray CameraRay = _playerCam.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(CameraRay, out RaycastHit info, 100f))
            {
                _currentTower.transform.position = info.point;
            }

            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(1))
            {
                _currentTower = null;
            }
        }
    }

    public void PlaceTower(GameObject Tower) 
    {
        _currentTower = Instantiate(Tower, Vector3.zero, Quaternion.identity);
    }
}
