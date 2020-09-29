using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

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
    //public Vector3 cameraOffsets = Vector3.zero;
    public Vector3 cameraOffsets = new Vector3(0, 0, -5);
    // Not implemented yet
    public float hideCharacterDistance = 1.0f;

    [Header("Movement")]
    public float centerHeight;
    public float maxAngle;

    public float maxSpeed;
    public float groundAcceleration;
    public float airAcceleration;
    public float groundFriction;
    public float airFriction;
    public Vector3 gravity = new Vector3(0.0f, -9.8f, 0.0f);

    public float jumpSpeed;
    public int maxJumps;
    public bool countGroundJump;
    public float jumpLandingLag;

    [Header("Character")]
    public float health;

    [Header("Projectile")]
    public float projectileSpeed;

    private bool prevOnGround = false;
    private bool onGround = false;
    private float groundEpsilon = 1e-1f;
    private Vector3 groundNormal = Vector3.zero;
    private GameObject ground;
    private Vector3 groundLastPosition;
    private Quaternion groundLastRotation;

    private bool holdJump = false;
    private int jumpCounter = 0;
    private float onGroundTime = 0;

    private Vector2 moveAxis = Vector3.zero;
    private Vector2 lookAxis = Vector3.zero;
    private Vector3 cameraEulerAngles = Vector3.zero;

    new Rigidbody rigidbody;

    private PlayerInputActions inputs;

    private void Awake()
    {
        // Connect input events to callbacks

        inputs = new PlayerInputActions();

        inputs.World.Move.started += OnMove;
        inputs.World.Move.performed += OnMove;
        inputs.World.Move.canceled += OnMove;

        inputs.World.Look.performed += OnLook;

        inputs.World.Jump.started += OnJump;
        inputs.World.Jump.performed += OnJump;
        inputs.World.Move.canceled += OnJump;

        inputs.World.Use.performed += OnUse;

        rigidbody = GetComponent<Rigidbody>();
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

    private void FixedUpdate()
    {
        rigidbody.AddForce(gravity, ForceMode.Acceleration);

        // Movement

        Accelerate();

        // Jump check

        if (holdJump && onGround)
        {
            Jump();
        }

        // Ground check

        prevOnGround = onGround;
        onGround = GetGrounded();

        if (onGround)
        {
            jumpCounter = 0;

            if (prevOnGround)
            {
                onGroundTime += Time.fixedDeltaTime;
            }

            if (ground)
            {
                groundLastPosition = ground.transform.position;
                groundLastRotation = ground.transform.rotation;
            }
        }
    }

    private void Accelerate()
    {
        // Get movement direction

        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;

        forward.y = 0.0f;
        right.y = 0.0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = (forward * moveAxis.y) + (right * moveAxis.x);
        moveDir.y = 0.0f;

        float moveSpeed = moveDir.magnitude;
        moveDir.Normalize();

        Vector3 prevVelocity = rigidbody.velocity;
        float speed = prevVelocity.magnitude;

        // Friction

        float friction = onGround ? groundFriction : airFriction;
        prevVelocity = Vector3.Lerp(prevVelocity, Vector3.zero, friction * Time.fixedDeltaTime);

        // Movement with ground

        if (ground && prevOnGround)
        {
            Vector3 dv = ground.transform.position - groundLastPosition;
            //Vector3 dr = ground.transform.rotation.eulerAngles - groundLastRotation.eulerAngles;

            rigidbody.MovePosition(rigidbody.position + dv);
            //rigidbody.MoveRotation(Quaternion.Euler(rigidbody.rotation.eulerAngles + dr));
        }

        // Movement

        float projectedSpeed = Vector3.Dot(prevVelocity, moveDir);
        float accMagnitude = onGround ? groundAcceleration : airAcceleration;
        accMagnitude *= Time.fixedDeltaTime;

        if (projectedSpeed + accMagnitude > maxSpeed)
        {
            accMagnitude = maxSpeed - projectedSpeed;
        }

        rigidbody.velocity = prevVelocity + (moveDir * accMagnitude);
    }

    private void Jump()
    {
        if (onGroundTime < jumpLandingLag || jumpCounter >= maxJumps)
        {
            return;
        }

        if (countGroundJump || !onGround)
        {
            ++jumpCounter;
        }

        Vector3 newVelocity = rigidbody.velocity;

        newVelocity.y = jumpSpeed;

        rigidbody.velocity = newVelocity;

        onGround = false;
    }

    public bool GetGrounded()
    {
        groundNormal = Vector3.zero;
        ground = null;

        /*if (rigidbody.velocity.y > 0.0f)
        {
            return false;
        }*/

        RaycastHit hitResult;
        //bool hasHit = Physics.Raycast(transform.position, Vector3.down, out hitResult);
        bool hasHit = Physics.SphereCast(transform.position, 0.5f, Vector3.down, out hitResult);

        if (hasHit)
        {
            float angle = Vector3.Angle(Vector3.up, hitResult.normal);

            if (Mathf.Abs(angle) <= maxAngle && Mathf.Abs(hitResult.distance - centerHeight + 0.5f) <= groundEpsilon)
            {
                groundNormal = hitResult.normal;
                ground = hitResult.collider.gameObject;

                return true;
            }
        }

        return false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputAxis = context.ReadValue<Vector2>();

        moveAxis = inputAxis.normalized;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            holdJump = true;

            Jump();
        }
        else if (context.performed && !context.control.IsPressed())
        {
            holdJump = false;
        }
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
        //Debug.Log("offset " + offset);


        offset.y = 0.0f;
        offset.Normalize();
        //Debug.Log("after offset " + offset);


        Projectile projectile = instance.GetComponent<Projectile>();

        projectile.position = transform.position + offset;
        //Debug.Log("proj speed " + projectileSpeed);

        //Debug.Log("setting to " + offset * projectileSpeed);

        projectile.velocity = offset * projectileSpeed;
    }
}
