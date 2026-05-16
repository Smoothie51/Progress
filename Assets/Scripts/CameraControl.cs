using UnityEngine;
using UnityEngine.InputSystem;

public class EarthCameraController : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform earthModel;

    [Header("Input Actions")]
    public InputActionReference mousePanning;
    public InputActionReference zoomAction;

    [Header("Zoom Settings")]
    public float zoomSpeed = 0.1f; // Input system scroll values are larger, so lower this
    public float minZoom = 2f;
    public float maxZoom = 10f;

    [Header("Orbit Settings")]
    public float lookSensitivity = 0.2f;
    public bool stickToEarthRotation = false;

    private float currentZoom;
    private Vector2 rotationInput;
    private float scrollInput;

    void OnEnable()
    {
        mousePanning.action.Enable();
            mousePanning.action.performed += MousePanning;
        zoomAction.action.Enable();
            zoomAction.action.performed += HandleZoom;
    }

    void OnDisable()
    {
        mousePanning.action.Disable();
            mousePanning.action.performed -= MousePanning;
        zoomAction.action.Disable();
            zoomAction.action.performed -= HandleZoom;
    }

    void Start()
    {
        if (cam != null)
            currentZoom = Vector3.Distance(cam.localPosition, Vector3.zero);
    }

    void Update() // Use LateUpdate for Cameras to prevent jitter
    {
        HandleRotation(); 
    }

    void HandleZoom(InputAction.CallbackContext context)
    {
        scrollInput = context.ReadValue<Vector2>().y;
        
        if (Mathf.Abs(scrollInput) > 0.01f)
        {
            // Input System scroll values are usually ~120 per notch
            currentZoom -= scrollInput * zoomSpeed * Time.deltaTime;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
            cam.localPosition = new Vector3(0, 0, -currentZoom);
        }
    }

    void HandleRotation()
    {
        // Option A: Locked to Earth rotation
        if (stickToEarthRotation && earthModel != null)
        {
            transform.rotation = earthModel.rotation;
        }
    }

    void MousePanning(InputAction.CallbackContext context)
    {
        Vector2 delta = context.ReadValue<Vector2>();

        float mouseX = delta.x * lookSensitivity;
        float mouseY = delta.y * lookSensitivity;

        transform.Rotate(Vector3.up, mouseX, Space.World);
        transform.Rotate(Vector3.right, -mouseY, Space.Self);
    }
}