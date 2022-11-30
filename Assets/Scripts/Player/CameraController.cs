using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject CameraRotator;
    public Camera MainCamera;

    public float rotationSpeed = 5;
    public float distance;
    public int cameraRotation = 1;

    public GameObject followObject;
    public float followSpeed;

    private void Awake()
    {
        CameraRotator.transform.position = followObject.transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown("a"))
        {
            cameraRotation += 1;
        }else if (Input.GetKeyDown("d"))
        {
            cameraRotation -= 1;
        }
        
        float interpolation = followSpeed * Time.deltaTime;

        Vector3 position = CameraRotator.transform.position;
        Vector3 followPosition = followObject.transform.position;
        
        position.y = Mathf.Lerp(position.y, followPosition.y, interpolation);
        position.x = Mathf.Lerp(position.x, followPosition.x, interpolation);
        position.z = Mathf.Lerp(position.z, followPosition.z, interpolation);

        CameraRotator.transform.position = position;

        var rotation = transform.rotation;
        
        CameraRotator.transform.rotation = Quaternion.Lerp(rotation, Quaternion.Euler(
            rotation.x, cameraRotation * 60, rotation.z), Time.deltaTime * rotationSpeed);

        MainCamera.transform.localPosition = new Vector3(distance * 10 + 10, distance * 20, 0);
        MainCamera.transform.LookAt(CameraRotator.transform.position);
    }
}