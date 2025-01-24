using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherTowerUpgrade : MonoBehaviour
{
    [SerializeField] private GameObject _dualLauncher;
    public void UpgradeTurret()
    {
        GameObject turret = Instantiate(_dualLauncher, transform);
        turret.transform.parent = null;
        Destroy(gameObject);
    }
}
