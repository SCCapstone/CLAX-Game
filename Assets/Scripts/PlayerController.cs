using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;
/*
 * Controls the player attacks, normal movement, and cheat mode movement
 */
public class PlayerController : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject player;
    public Material cBGreen;
    public Camera playerCamera;
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;

    [Header("Camera")]
    public bool cameraEnabled = true;

    public float lookSensitivityScale = Globals.mouseSettings.mouseSensitivity;
    public float lookSensitivityX = 0.6f;
    public float lookSensitivityY = 0.6f;

    public float minPitch = -89.9f;
    public float maxPitch = 89.9f;

    public Vector3 cameraTargetOffset = Vector3.zero;
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
    public float dieAtY;

    [Header("Projectiles")]
    public bool holdToShoot;
    public float shootDelay;
    public float projectileDamage;
    public float projectileSpeed;
    public int maxExplosionCount;

    [Header("Audio")]
    public AudioSource playerShootSound;
    public AudioSource playerSecondaryShootSound;
    public AudioSource playerJumpSound;

    [Header("Test Mode")]
    public float flyingAirFriction;
    public float flyingAirSpeed;
    public float flyingElevationSpeed;
    public float fastFlyingAirSpeed;
    public float fastFlyingElevationSpeed;

    private bool isTestMode = false;
    private bool isAscending = false;
    private bool isDescending = false;
    private bool isFlyingFast = false;
    private bool isElevationCoroutineRunning = false;

    private bool prevOnGround = false;
    private bool onGround = false;
    private float groundEpsilon = 1e-1f;
    private GameObject ground;
    private Vector3 groundLastPosition;

    private bool holdJump = false;
    private int jumpCounter = 0;
    private float onGroundTime = 0;

    private bool holdUse = false;
    private Coroutine shootLoop;

    private Vector2 moveAxis = Vector3.zero;
    private Vector3 cameraEulerAngles = Vector3.zero;

    new Rigidbody rigidbody;

    private PlayerInputActions inputs;

    private void Awake()
    {
        // Event callbacks

        inputs = new PlayerInputActions();

        inputs.World.Move.started += OnMove;
        inputs.World.Move.performed += OnMove;
        inputs.World.Move.canceled += OnMove;

        inputs.World.Jump.started += OnJump;
        inputs.World.Jump.performed += OnJump;
        inputs.World.Move.canceled += OnJump;

        inputs.World.Use.performed += OnUse;

        inputs.World.Explosion.performed += OnRightClick;

        inputs.World.TestMode.performed += OnToggleTestMode;

        inputs.World.FlyUp.started += OnAscend;
        inputs.World.FlyUp.performed += OnAscend;
        inputs.World.FlyUp.canceled += OnAscend;

        inputs.World.FlyDown.started += OnDescend;
        inputs.World.FlyDown.performed += OnDescend;
        inputs.World.FlyDown.canceled += OnDescend;

        inputs.World.FlyFast.started += OnFlyFast;
        inputs.World.FlyFast.performed += OnFlyFast;
        inputs.World.FlyFast.canceled += OnFlyFast;

        // Rigidbody physics

        rigidbody = GetComponent<Rigidbody>();

        // Pause menu

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Hacky color change

        if (Globals.colorBlindEnabled)
        {
            player.transform.GetChild(0).gameObject.GetComponent<Renderer>().material = cBGreen;
        }
    }

    private void Start()
    {
        AliveObject aliveObject = GetComponentInChildren<AliveObject>();

        if (aliveObject != null)
        {
            aliveObject.onDeath += RespawnPlayer;
        }
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
        playerCamera.fieldOfView = Globals.videoSettings.fieldOfView;
        QualitySettings.vSyncCount = Globals.videoSettings.vsyncEnabled ? 1 : 0;
        lookSensitivityScale = Globals.mouseSettings.mouseSensitivity;

        if (!cameraEnabled || !Application.isFocused || Globals.IsPaused())
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector2 mouseDelta = inputs.World.Look.ReadValue<Vector2>();
        float dPitch = mouseDelta.y * lookSensitivityY;
        float dYaw = mouseDelta.x * lookSensitivityX;

        /*Mouse mouse = Mouse.current;
        Vector2 mouseDelta = mouse.delta.ReadValue();
        float dPitch = -mouseDelta.y * lookSensitivityY;
        float dYaw = mouseDelta.x * lookSensitivityX;*/

        cameraEulerAngles += new Vector3(dPitch, dYaw, 0) * lookSensitivityScale;
        cameraEulerAngles.x = Mathf.Clamp(cameraEulerAngles.x, minPitch, maxPitch);

        // Set rotation before performing the following steps
        playerCamera.transform.rotation = Quaternion.Euler(cameraEulerAngles);

        Vector3 cameraTarget = transform.position + cameraTargetOffset;
        Vector3 offsetX = playerCamera.transform.right * cameraOffsets.x;
        Vector3 offsetY = playerCamera.transform.up * cameraOffsets.y;
        Vector3 offsetZ = playerCamera.transform.forward * cameraOffsets.z;

        int layerMasks = 1;

        bool hasHit = Physics.Raycast(cameraTarget, offsetZ, out RaycastHit hit, Mathf.Abs(cameraOffsets.z), layerMasks);

        playerCamera.transform.position = hasHit ? hit.point : cameraTarget + offsetX + offsetY + offsetZ;

        CheckY();
    }

    private void FixedUpdate()
    {
        if (Globals.IsPaused())
        {
            StopCoroutine(shootLoop);
        }

        if (!isTestMode)
        {
            rigidbody.AddForce(gravity, ForceMode.Acceleration);
        }

        // Jump check

        if (holdJump && onGround)
        {
            Jump();
        }

        // Movement

        Accelerate();

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
                //groundLastRotation = ground.transform.rotation;
            }
        }
    }

    void RespawnPlayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    /*
     * Check if player has fallen off edge
     */
    private void CheckY()
    {
        if (transform.position.y < dieAtY)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

        if (isTestMode)
        {
            friction = flyingAirFriction;
        }

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

        if (isTestMode)
        {
            accMagnitude = (isFlyingFast ? fastFlyingAirSpeed : flyingAirSpeed) * Time.fixedDeltaTime;
        }
        else
        {
            accMagnitude *= Time.fixedDeltaTime;

            if (projectedSpeed + accMagnitude > maxSpeed)
            {
                accMagnitude = maxSpeed - projectedSpeed;
            }
        }

        rigidbody.velocity = prevVelocity + (moveDir * accMagnitude);
    }
    /*
     * Move the player up when they press space
     */
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

        playerJumpSound.volume = Globals.audioSettings.gameVolume;
        playerJumpSound.Play();
    }

    /*
     * Check if player is on ground
     */
    public bool GetGrounded()
    {
        //groundNormal = Vector3.zero;
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
                //groundNormal = hitResult.normal;
                ground = hitResult.collider.gameObject;

                return true;
            }
        }

        return false;
    }
    /*
     * Move the player left right forward and backward
     */
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputAxis = context.ReadValue<Vector2>();

        moveAxis = inputAxis.normalized;
    }
    /*
     * Move the player up when they jump
     */
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!isTestMode)
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
    }
    /*
     * Fire primary attack
     */
    public void OnUse(InputAction.CallbackContext context)
    {
        if (Globals.IsPaused())
        {
            if (shootLoop != null)
            {
                StopCoroutine(shootLoop);
            }
            return;
        }

        bool pressed = context.control.IsPressed();

        if (holdToShoot)
        {
            if (pressed)
            {
                if (shootLoop != null)
                {
                    StopCoroutine(shootLoop);
                }

                holdUse = true;
                shootLoop = StartCoroutine(ShootLoop());
            }
            else if (shootLoop != null)
            {
                StopCoroutine(shootLoop);
                holdUse = false;
            }
        }
        else if (pressed)
        {
            FireProjectile();
        }
    }
    /*
     * Fire secondary attack
     */
    public void OnRightClick(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
        {
            return;
        }

        if (Globals.IsPaused())
        {
            return;
        }

        int existingCount = GameObject.FindGameObjectsWithTag("ExplosionAttack").Length;

        if (existingCount >= maxExplosionCount)
        {
            return;
        }

        Explosion instance = Instantiate(explosionPrefab).GetComponent<Explosion>();

        // Get horizontal facing vector
        Vector3 facing = Vector3.ProjectOnPlane(playerCamera.transform.forward, Vector3.up);
        facing.Normalize();

        instance.transform.position = transform.position + facing;

        playerSecondaryShootSound.volume = Globals.audioSettings.gameVolume;
        playerSecondaryShootSound.Play();
    }
    /*
     * Fires a player projectile in the direction they are looking
     */
    void FireProjectile()
    {
        // Get horizontal facing vector
        Vector3 facing = Vector3.ProjectOnPlane(playerCamera.transform.forward, Vector3.up);
        facing.Normalize();

        PlayerProjectile projectile = Instantiate(bulletPrefab).GetComponent<PlayerProjectile>();

        projectile.position = transform.position;
        projectile.velocity = facing * projectileSpeed;

        projectile.damage = isTestMode ? float.MaxValue : projectileDamage;

        // Offset initial position slightly so any first person view doesn't look janky when
        // projectile is instantiated inside their camera
        // Preferably, we would want it to originate from the center and only make the projectile
        // appear visible slightly later, but this solution is fine for now.
        // TODO: Whatever it says above for posterity
        if (cameraOffsets.sqrMagnitude < Vector3.kEpsilon)
        {
            projectile.position += facing;
        }

        playerShootSound.volume = Globals.audioSettings.gameVolume;
        playerShootSound.Play();
    }
    /*
     * Continuously fire when the player holds down the left mouse button
     */
    IEnumerator ShootLoop()
    {
        while (holdUse)
        {
            FireProjectile();

            yield return new WaitForSeconds(shootDelay);
        }
    }
    /*
     * Enable or disable cheat (debug) mode
     */
    void OnToggleTestMode(InputAction.CallbackContext context)
    {
        isTestMode = !isTestMode;

        if (isTestMode)
        {
            gameObject.GetComponentInChildren<AliveObject>().invulnerable = true;

            rigidbody.useGravity = false;
        }
        else
        {
            gameObject.GetComponentInChildren<AliveObject>().invulnerable = false;

            rigidbody.useGravity = true;
        }
    }
    /*
     * Move player up
     */
    IEnumerator Elevate()
    {
        isElevationCoroutineRunning = true;

        float speed;

        while (isTestMode && (isAscending || isDescending))
        {
            speed = isFlyingFast ? fastFlyingElevationSpeed : flyingElevationSpeed;

            player.transform.position += (isAscending ? Vector3.up : Vector3.down) * speed * Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        isElevationCoroutineRunning = false;
    }
    /*
     * Move player up when in cheat mode
     */
    void OnAscend(InputAction.CallbackContext context)
    {
        isAscending = context.control.IsPressed();

        if (isTestMode && !isElevationCoroutineRunning)
        {
            StartCoroutine(Elevate());
        }
    }
    /*
     * Move player down when in cheat mode
     */
    void OnDescend(InputAction.CallbackContext context)
    {
        isDescending = context.control.IsPressed();

        if (isTestMode && !isElevationCoroutineRunning)
        {
            StartCoroutine(Elevate());
        }
    }
    /*
     * Move player quickly when in cheat mode and flying
     */
    void OnFlyFast(InputAction.CallbackContext context)
    {
        isFlyingFast = context.control.IsPressed();
    }
    /*
     * Simple camera angle variable changer
     */
    public void SetCameraAngles(Vector3 euler)
    {
        cameraEulerAngles = euler;
    }
}
