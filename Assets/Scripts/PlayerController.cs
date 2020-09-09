using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Camera playerCamera;

    public float lookSensitivityScale = 90.0f;
    public float lookSensitivityX = 0.6f;
    public float lookSensitivityY = 0.6f;

    public float minPitch = -89.9f;
    public float maxPitch = 89.9f;

    public bool cameraEnabled = true;
    public float cameraDistance = 0.0f;
    // Not implemented yet
    public float hideCharacterDistance = 1.0f;

    public float walkSpeed = 10.0f;

    public Vector3 velocity = Vector3.zero;
    public Vector3 gravity = new Vector3(0.0f, -9.8f, 0.0f);
    public float drag = 0.02f;

    public float projectileSpeed = 0.1f;

    public GameObject bulletPrefab;

    private Vector2 moveAxis = Vector3.zero;
    private Vector2 lookAxis = Vector3.zero;
    private Vector3 cameraEulerAngles = Vector3.zero;

    public float health = 100;

    private PlayerInputActions inputs;

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputAxis = context.ReadValue<Vector2>();

        moveAxis = inputAxis.normalized;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
        {
            return;
        }

        /*
         * TODO:
         * Check if on ground
         * Jump cooldown
         * Jump counter to support mid-air jumps (i.e. double jumping)
         */

        velocity += new Vector3(0.0f, 10.0f, 0.0f);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 inputAxis = context.ReadValue<Vector2>();

        lookAxis = inputAxis;
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
        {
            return;
        }

        //make a new bullet and initialize who the enemy is (9 is the enemy layer)
        Projectile instance = Instantiate(bulletPrefab).GetComponent<Projectile>();
        instance.Initialize(9);


        Vector3 offset = playerCamera.transform.forward;

        offset.y = 0.0f;
        offset.Normalize();

        Projectile projectile = instance.GetComponent<Projectile>();

        // TODO: Allow editor-friendly editing of projectile spawn position/orientation
        projectile.position = transform.position + offset;
        projectile.velocity = offset * projectileSpeed;

        //debug text not needed now that it works
        //Debug.Log("Projectile fired");
    }

    private void Awake()
    {
        inputs = new PlayerInputActions();
    }

    // Start is called before the first frame update
    void Start()
    {
        /*
         * TODO:
         * Initialize camera boom
         * Initialize player controls if necessary
         */
    }

    void UpdateCamera()
    {
        if (!cameraEnabled || !Application.isFocused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        float dPitch = lookAxis.y * lookSensitivityY;
        float dYaw = lookAxis.x * lookSensitivityX;

        cameraEulerAngles += new Vector3(dPitch, dYaw, 0) * lookSensitivityScale * Time.deltaTime;
        cameraEulerAngles.x = Mathf.Clamp(cameraEulerAngles.x, minPitch, maxPitch);

        // Set rotation before performing the following steps
        playerCamera.transform.rotation = Quaternion.Euler(cameraEulerAngles);

        Vector3 focus = transform.position;
        Vector3 cameraBoomDirection = -playerCamera.transform.forward;

        RaycastHit hit;
        bool hasHit = Physics.Raycast(focus, cameraBoomDirection, out hit, cameraDistance);

        playerCamera.transform.position = hasHit ? hit.point : focus + (cameraBoomDirection * cameraDistance);

        /*
         * TODO:
         * Let the camera distance/boom be configurable
         */
    }

    void UpdatePosition()
    {
        Vector3 moveVector = Vector3.zero;

        moveVector += playerCamera.transform.forward * moveAxis.y;
        moveVector += playerCamera.transform.right * moveAxis.x;
        moveVector.y = 0.0f;
        moveVector.Normalize();

        moveVector *= walkSpeed;

        velocity.x = moveVector.x;
        velocity.z = moveVector.z;

        /*
         * Rudimentary gravity and raycast check
         */

        Vector3 nextPosition = transform.position + (velocity * Time.fixedDeltaTime) + (gravity * 0.5f * Mathf.Pow(Time.fixedDeltaTime, 2.0f));
        Vector3 nextVelocity = (velocity * (1.0f - drag)) + (gravity * Time.fixedDeltaTime);

        RaycastHit hit;

        bool hasHit = Physics.Raycast(nextPosition + Vector3.up, Vector3.down, out hit, 2.0f);

        if (hasHit)
        {
            nextVelocity.y = 0.0f;
            velocity = nextVelocity;
            transform.position = hit.point + new Vector3(0.0f, 1.0f, 0.0f);
        }
        else
        {
            velocity = nextVelocity;
            transform.position = nextPosition;
        }
    }

    private void Update()
    {
        // Update every render frame
        UpdateCamera();
    }

    void FixedUpdate()
    {
        // Update every physics frame
        UpdatePosition();
    }
}
