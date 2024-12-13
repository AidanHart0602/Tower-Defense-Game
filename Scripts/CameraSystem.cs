using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    
    public float camSpeed;


    // Update is called once per frame
    void Update()
    {
        if (transform.position.z > 9.5f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 9.5f);
            return;
        }
        else if (transform.position.z < -5)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -5f);
            return;
        }
        else if (transform.position.x > -18)
        {
            transform.position = new Vector3(-18, transform.position.y, transform.position.z);
            return;
        }
        else if (transform.position.x < -33)
        {
            transform.position = new Vector3(-33, transform.position.y, transform.position.z);
            return;
        }
        CamMovement();
        CamRotation();
    }

    private void CamMovement()
    {
        Vector3 inputDirection = new Vector3(0, 0, 0);
      
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            inputDirection.z = 1;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            inputDirection.z = -1;
        }

        Vector3 CamDirection = transform.forward * inputDirection.z + transform.right * inputDirection.x;

        transform.position += CamDirection * camSpeed * Time.deltaTime;
    }

    void CamRotation()
    {
        float camRotation = 0;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            camRotation += -1;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            camRotation += 1;
        }

        Vector3 NewRotation = new Vector3(0, camRotation * 300 * Time.deltaTime, 0);

        transform.eulerAngles -= NewRotation;
    }
}
