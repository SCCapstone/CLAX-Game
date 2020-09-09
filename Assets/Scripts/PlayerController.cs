using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Prefabs")]
    public Camera playerCamera;
    public GameObject bulletPrefab;

    [Header("Camera")]
    public bool cameraEnabled = true;

    public float lookSensitivityScale = 90.0f;
    public float lookSensitivityX = 0.6f;
    public float lookSensitivityY = 0.6f;

    public float minPitch = -89.9f;
    public float maxPitch = 89.9f;

    public Vector3 cameraTargetOffset = Vector3.zero;
    public Vector3 cameraOffsets = Vector3.zero;
    // Not implemented yet
    public float hideCharacterDistance = 1.0f;

    [Header("Movement")]
    public float centerHeight = 1.0f;

    public float walkSpeed = 10.0f;
    public Vector3 gravity = new Vector3(0.0f, -9.8f, 0.0f);
    public float drag = 0.02f;

    public float jumpSpeed = 10.0f;
    public int maxJumps = 1;
    public bool countGroundJump = false;
    public float jumpLandingLag = 0.03f;

    [Header("Projectile")]
    public float projectileSpeed = 25.0f;

    private Vector3 velocity = Vector3.zero;
    private bool onGround = true;
    private Vector3 groundNormal = Vector3.zero;
    private float lastLandingTime = 0;
    private int jumpCounter = 0;

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
        if (jumpCounter >= maxJumps || Time.time - lastLandingTime < jumpLandingLag)
        {
            return;
        }

        if (countGroundJump || !onGround)
        {
            ++jumpCounter;
        }

        velocity.y = jumpSpeed;
        onGround = false;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 inputAxis = context.ReadValue<Vector2>();

        lookAxis = inputAxis;
    }

    public void OnUse(InputAction.CallbackContext context)
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

        projectile.position = transform.position + offset;
        projectile.velocity = offset * projectileSpeed;
    }

    private void Awake()
    {
        // Connect input events to callbacks

        inputs = new PlayerInputActions();

        inputs.World.Move.started += OnMove;
        inputs.World.Move.performed += OnMove;
        inputs.World.Move.canceled += OnMove;

        inputs.World.Look.performed += OnLook;

        inputs.World.Jump.performed += OnJump;
        inputs.World.Use.performed += OnUse;
    }

    private void OnEnable()
    {
        inputs.Enable();
    }

    private void OnDisable()
    {
        inputs.Disable();
    }

    private void Update()
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

        Vector3 cameraTarget = transform.position + cameraTargetOffset;
        Vector3 offsetX = playerCamera.transform.right * cameraOffsets.x;
        Vector3 offsetY = playerCamera.transform.up * cameraOffsets.y;
        Vector3 offsetZ = playerCamera.transform.forward * cameraOffsets.z;

        RaycastHit hit;
        bool hasHit = Physics.Raycast(cameraTarget, offsetZ, out hit, Mathf.Abs(cameraOffsets.z));

        playerCamera.transform.position = hasHit ? hit.point : cameraTarget + offsetX + offsetY + offsetZ;
    }

    void FixedUpdate()
    {
        // Move player

        Vector3 forward = playerCamera.transform.forward * moveAxis.y;
        Vector3 right = playerCamera.transform.right * moveAxis.x;
        Vector3 moveVector = forward + right;

        moveVector.y = 0.0f;
        moveVector.Normalize();

        if (onGround && groundNormal != Vector3.zero)
        {
            moveVector = Vector3.ProjectOnPlane(moveVector, groundNormal);
        }

        moveVector *= walkSpeed;

        /*
         * Rudimentary gravity and raycast check
         */

        Vector3 realizedVelocity = velocity + moveVector;

        Vector3 nextPosition = transform.position + (realizedVelocity * Time.fixedDeltaTime) + (gravity * 0.5f * Mathf.Pow(Time.fixedDeltaTime, 2.0f));
        Vector3 nextVelocity = (velocity * (1.0f - drag)) + (gravity * Time.fixedDeltaTime);

        RaycastHit hit;
        bool hasHit = Physics.Raycast(nextPosition, Vector3.down, out hit, centerHeight);

        if (hasHit)
        {
            nextVelocity.y = 0.0f;
            transform.position = hit.point + (Vector3.up * centerHeight);

            // Restore jumps on ground
            jumpCounter = 0;

            // Initiate landing lag if landed
            if (!onGround)
            {
                lastLandingTime = Time.time;
            }
        }
        else
        {
            transform.position = nextPosition;
        }

        velocity = nextVelocity;
        onGround = hasHit;
        groundNormal = hasHit ? hit.normal : Vector3.zero;
    }
}
