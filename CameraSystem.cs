using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraSystem : MonoBehaviour
{
    
    public float camSpeed, rotationSpeed;

    [SerializeField] private CinemachineVirtualCamera _virtualCam;
    private float _fovMin = 10, _fovMax = 50, _fov = 50;
    
    void Update()
    {
        CamMovement();
        CamRotation();
        ZoomInAndOut();
    }

    private void CamMovement()
    {
        Vector3 inputDirection = new Vector3(0, 0, 0);
      
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            inputDirection.z = 1;
        }

        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            inputDirection.z = -1;
        }

        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            inputDirection.x = -1;
        }

        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            inputDirection.x = 1;
        }

        Vector3 CamDirection = transform.forward * inputDirection.z + transform.right * inputDirection.x;

        transform.position += CamDirection * camSpeed * Time.deltaTime;

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
    }

    void CamRotation()
    {
        float camRotation = 0;

        if (Input.GetKey(KeyCode.Q))
        {
            camRotation += -1;
        }

        if (Input.GetKey(KeyCode.E))
        {
            camRotation += 1;
        }
        Vector3 NewRotation = new Vector3(0, camRotation * rotationSpeed * Time.deltaTime, 0);

        transform.eulerAngles -= NewRotation;
    }

    void ZoomInAndOut()
    {
        if(Input.mouseScrollDelta.y > 0)
        {
            _fov += -5;
        }

        if(Input.mouseScrollDelta.y < 0)
        {
            _fov += 5;
        }
        _fov = Mathf.Clamp(_fov, _fovMin, _fovMax);
        _virtualCam.m_Lens.FieldOfView = _fov;
    }


}
