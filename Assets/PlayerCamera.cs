using UnityEngine;

public class TPPCamera : MonoBehaviour
{
    [Header("Cel do śledzenia")]
    public Transform target;

    [Header("Ustawienia dystansu")]
    public float distance = 5.0f;
    public float minDistance = 2.0f;
    public float maxDistance = 10.0f;
    public float zoomSpeed = 2.0f;

    [Header("Ustawienia obrotu (Myszka)")]
    public float mouseSensitivity = 200f;
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    [Header("Wygładzanie (Opcjonalne)")]
    public bool smoothMovement = true;
    public float smoothTime = 0.12f;

    private float x = 0.0f;
    private float y = 0.0f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPosition;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (!target) return;


        x += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        y -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        y = Mathf.Clamp(y, yMinLimit, yMaxLimit);

        distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, minDistance, maxDistance);


        Quaternion rotation = Quaternion.Euler(y, x, 0);


        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + target.position;

        transform.rotation = rotation;

        if (smoothMovement)
        {
            transform.position = Vector3.SmoothDamp(transform.position, position, ref velocity, smoothTime);
        }
        else
        {
            transform.position = position;
        }
    }
}