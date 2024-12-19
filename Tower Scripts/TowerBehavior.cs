using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Placement"))
        {
            Debug.Log("Connected to Placement");
            transform.parent = other.transform;
            transform.position = Vector3.zero;
        }
    }
}
