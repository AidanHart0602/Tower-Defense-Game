using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatlingTowerUpgrade : MonoBehaviour
{
    [SerializeField]private GameObject _dualTurret;

    public void UpgradeTurret()
    {
        Debug.Log("Upgraded Tower");
        GameObject turret = Instantiate(_dualTurret, transform);
        turret.transform.parent = null;
        Destroy(gameObject);
    }
}
