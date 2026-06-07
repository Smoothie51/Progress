using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class EarthCameraController : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform earthModel;

    [Header("Input Actions")]
    public InputActionReference mousePanning;
    public InputActionReference zoomAction;

    [Header("Zoom Settings")]
    public float zoomSpeed = 0.1f; 
    public float minZoom = 12f; // Adjusted assuming Earth scale is 10
    public float maxZoom = 40f;

    [Header("Orbit Settings")]
    public float lookSensitivity = 0.2f;
    public bool stickToEarthRotation = false;

    private float currentZoom;
    
    // Store the player's custom camera offsets explicitly
    private float panningYaw = 0f;
    private float panningPitch = 0f;

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
            
        // Initialize offsets with current transform values so the camera doesn't snap on game start
        panningYaw = transform.localEulerAngles.y;
        panningPitch = transform.localEulerAngles.x;
    }

    // Always use LateUpdate for cameras to prevent frame stuttering against moving objects
    void LateUpdate() 
    {
        HandleRotationAndTracking(); 
    }

    void HandleZoom(InputAction.CallbackContext context)
    {
        if (!(EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject())) 
            return;
        float scrollInput = context.ReadValue<Vector2>().y;
        
        if (Mathf.Abs(scrollInput) > 0.01f)
        {
            // Removed Time.deltaTime here because InputSystem scroll contexts fire discretely 
            currentZoom -= scrollInput * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
            cam.localPosition = new Vector3(0, 0, -currentZoom);
        }
    }

    void MousePanning(InputAction.CallbackContext context)
    {
        if (!(EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject())) 
            return;
        Vector2 delta = context.ReadValue<Vector2>();

        // Accumulate rotation modifications safely into our tracking variables
        panningYaw += delta.x * lookSensitivity;
        panningPitch -= delta.y * lookSensitivity; // Inverted so dragging up tilts up

        // Clamp vertical looking so the player can't flip the camera upside down over the poles
        panningPitch = Mathf.Clamp(panningPitch, -85f, 85f);
    }

    void HandleRotationAndTracking()
    {
        // 1. Create the rotation calculation relative to our custom pan settings
        Quaternion customPanRotation = Quaternion.Euler(panningPitch, panningYaw, 0f);

        if (stickToEarthRotation && earthModel != null)
        {
            // Option A: Use the Earth's current spinning angle as the BASE, then multiply our panning offset on top
            transform.rotation = earthModel.rotation * customPanRotation;
        }
        else
        {
            // Option B: Standard free-orbit mode (Independent of the Earth spinning under it)
            transform.rotation = customPanRotation;
        }
    }
}