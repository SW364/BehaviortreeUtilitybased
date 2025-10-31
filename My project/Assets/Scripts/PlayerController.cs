using UnityEngine;
using Game.Interactions;

[RequireComponent(typeof(CharacterController))]
public class PlayerControllerMouseLookManualPivot : MonoBehaviour
{
    [Header("Refs (setear manualmente en escena)")]
    public Camera cam;                 // Asigna Main Camera
    public Transform cameraPivot;      // Asigna el CameraPivot hijo del Player

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 1.5f;
    public float minPitch = -35f;
    public float maxPitch = 70f;
    public bool lockCursor = true;

    [Header("Interaction")]
    public float interactRange = 2.5f;
    public KeyCode interactKey = KeyCode.E;
    public LayerMask interactableMask = ~0;   // Everything

    CharacterController controller;
    float yaw;      // rotación Y del player
    float pitch;    // rotación X del pivot
    float yVel;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (cam == null) cam = Camera.main;

        // Validaciones
        if (cam == null) Debug.LogError("Asigna la Camera en el PlayerController.");
        if (cameraPivot == null) Debug.LogError("Asigna un CameraPivot (hijo del Player).");

        yaw = transform.eulerAngles.y;
        pitch = 0f;

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleInteraction();
    }

    void HandleMouseLook()
    {
        float mx = Input.GetAxis("Mouse X") * mouseSensitivity;
        float my = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mx;
        pitch = Mathf.Clamp(pitch - my, minPitch, maxPitch);

        // Yaw en el Player (gira el cuerpo)
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);

        // Pitch solo en el pivot (no inclina al cuerpo)
        if (cameraPivot != null)
            cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    void HandleMovement()
    {
        // Movimiento WASD relativo a la orientación de la cámara (sin pitch)
        Vector3 fwd = cam.transform.forward; fwd.y = 0f; fwd.Normalize();
        Vector3 right = cam.transform.right; right.y = 0f; right.Normalize();

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 moveDir = (fwd * v + right * h);
        if (moveDir.sqrMagnitude > 1f) moveDir.Normalize();

        if (controller.isGrounded && yVel < 0f) yVel = -2f;
        yVel += gravity * Time.deltaTime;

        Vector3 vel = moveDir * moveSpeed;
        vel.y = yVel;

        controller.Move(vel * Time.deltaTime);
    }

    void HandleInteraction()
    {
        if (!Input.GetKeyDown(interactKey) || cam == null) return;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableMask, QueryTriggerInteraction.Collide))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                interactable.Interact(transform);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (cam == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(cam.transform.position, cam.transform.position + cam.transform.forward * interactRange);
    }
}
