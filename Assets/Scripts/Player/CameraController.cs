using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject CameraRotator;
    public Camera MainCamera;

    public float rotationSpeed = 5;
    public float distance;
    public int cameraRotation = 1;

    private void Update()
    {
        CameraRotator.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(
            transform.rotation.x, cameraRotation * 60, transform.rotation.z), Time.deltaTime * rotationSpeed);

        MainCamera.transform.localPosition = new Vector3(distance * 10 + 10, distance * 20, 0);
        MainCamera.transform.LookAt(CameraRotator.transform.position);
    }
}