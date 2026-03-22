using UnityEngine;
using UnityEngine.EventSystems;

public class CameraManager : MonoBehaviour
{
    [Header("Pan")]
    [SerializeField] private float panSpeed = 1f;

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 1f;
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 12f;

    [SerializeField] private CharacterClickHandler clickHandler;

    private Camera cam;
    private Vector3 dragOrigin;
    public bool isPanning;


    private void Awake() => cam = GetComponent<Camera>();

    private void LateUpdate()
    {
        HandleZoom();
        HandlePan();
    }

    public void HandlePan()
    {
        if (isPanning)
        {
            if (Input.GetMouseButtonDown(0))
            {
                dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
                transform.position += difference * panSpeed;
            }

            if (Input.GetMouseButtonUp(0))
            {
                isPanning = false;
            }

        }
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll == 0f) return;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - scroll * zoomSpeed, minZoom, maxZoom);
    }
}