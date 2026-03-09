using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Pan")]
    [SerializeField] private float panSpeed = 1f;

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 1f;
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 12f;

    private Camera cam;
    private Vector3 dragOrigin;

    private void Awake() => cam = GetComponent<Camera>();

    private void Update()
    {
        HandlePan();
        HandleZoom();
    }

    private void HandlePan()
    {
        if (Input.GetMouseButtonDown(1))
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(1))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            transform.position += difference * panSpeed;
        }
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll == 0f) return;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - scroll * zoomSpeed, minZoom, maxZoom);
    }
}