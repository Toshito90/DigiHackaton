using System;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform target;

    public Transform Target
    {
        set
        {
            target = value;
        }

        get
        {
            return target;
        }
    }


    [Serializable]
    private class PositionSettings
    {
        public Vector3 targetPosOffset = new Vector3(0.0f, 3.4f, 0.0f);
        public float lookSmooth = 500.0f;
        public float distanceFromTarget = -8.0f;
        public float zoomSmooth = 700.0f;
        public float maxZoom = -2.0f;
        public float minZoom = -20.0f;
        public float zoomSpeed = 5.0f;
        public bool smoothFollow = true;
        public float smooth = 0.05f;

        [HideInInspector] public float newDistance = -8.0f;
        [HideInInspector] public float adjustmentDistance = -8.0f;
    }

    [Serializable]
    private class OrbitSettings
    {
        public float xRotation = -20.0f;
        public float yRotation = -180.0f;
        public float maxXRotation = 25.0f;
        public float minXRotation = -85.0f;
        public float vOrbitSmooth = 300.0f;
        public float hOrbitSmooth = 300.0f;
        public float initialYRotation = 180f;

        // Used for snapping
        public float snappingSpeed = 1f;
        [HideInInspector] public float snappingDuration = 0f;
        [HideInInspector] public float snappingPercentage = 0f;

        [HideInInspector] public float currentYRotation = -8.0f;
    }

    [Serializable]
    private class DebugSettings
    {
        public bool drawDesiredCollisionLines = true;
        public bool drawAdjustedCollisionLines = true;
    }

    [SerializeField] PositionSettings position = new PositionSettings();
    [SerializeField] OrbitSettings orbit = new OrbitSettings();
    [SerializeField] CollisionHandler collision = new CollisionHandler();
    [SerializeField] DebugSettings debug = new DebugSettings();
    public event Action onChangeTarget;

    Vector3 targetPos = Vector3.zero;
    Vector3 destination = Vector3.zero;
    Vector3 adjustedDestination = Vector3.zero;
    Vector3 cameraVelocity = Vector3.zero;

    float vOrbitInput, hOrbitInput, zoomInput, hOrbitSnapInput, yOrbitSnapInput;

    bool canControlCamera = true;

    // Start is called before the first frame update
    void Start()
    {
        vOrbitInput = hOrbitInput = zoomInput = hOrbitSnapInput = yOrbitSnapInput = 0.0f;

        SetCameraTarget(target);
        MoveToTarget();
        ResetLookOnTarget();

        collision.Initialize(Camera.main);
        collision.UpdateCameraClipPoints(transform.position, transform.rotation, ref collision.adjustedCameraClipPoints);
        collision.UpdateCameraClipPoints(destination, transform.rotation, ref collision.desiredCameraClipPoints);
    }

    private void Update()
    {
        ZoomOnTarget();
        CheckCollisions();

        // rotate
        LookAtTarget();

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            SetOrbitAxis(-Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X"));
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SetOrbitAxis();
        }

        // move
        MoveToTarget();

        // player input orbit
        OrbitTarget();

        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A))
		{
            ResetLookOnTarget();
		}
    }

    void FixedUpdate()
    {
        
    }

    /*private void LateUpdate()
    {
        // Snapping Movement
        //SnapMovement();
    }*/

    private void SnapMovement()
    {
        if (orbit.yRotation != orbit.initialYRotation)
        {
            if (hOrbitSnapInput > 0)
            {
                orbit.snappingDuration += Time.deltaTime;
                orbit.snappingPercentage = orbit.snappingDuration / orbit.snappingSpeed;

                //orbit.yRotation = EasingFunction.Linear(orbit.currentYRotation, orbit.initialYRotation, orbit.snappingPercentage);

                if (orbit.snappingPercentage >= 1f)
                {
                    orbit.snappingDuration = 0f;
                    orbit.snappingPercentage = 0f;
                    orbit.currentYRotation = 0f;

                    hOrbitSnapInput = 0f;
                }
            }
        }
    }

    void MoveToTarget()
    {
        if (targetPos != null && target != null)
        {
            targetPos = target.position + position.targetPosOffset;
            destination = Quaternion.Euler(orbit.xRotation, orbit.yRotation + target.localEulerAngles.y, 0.0f) * -Vector3.forward * position.distanceFromTarget;
            destination += targetPos;

            if (collision.isColliding)
            {
                adjustedDestination = Quaternion.Euler(orbit.xRotation, orbit.yRotation + target.localEulerAngles.y, 0.0f) * Vector3.forward * position.adjustmentDistance;
                adjustedDestination += targetPos;

                if (position.smoothFollow)
                {
                    // Use smooth damp function
                    transform.position = Vector3.SmoothDamp(transform.position, adjustedDestination, ref cameraVelocity, position.smooth);
                }
                else
                {
                    transform.position = adjustedDestination;
                }
            }
            else
            {
                if (position.smoothFollow)
                {
                    // Use smooth damp function
                    transform.position = Vector3.SmoothDamp(transform.position, destination, ref cameraVelocity, position.smooth);
                }
                else
                {
                    transform.position = destination;
                }
            }
        }
    }

    void LookAtTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetPos - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, position.lookSmooth * Time.deltaTime);
    }

    void OrbitTarget()
    {
        if (hOrbitSnapInput > 0)
        {
            ResetLookOnTarget();
            if (orbit.xRotation == -10.0f && orbit.yRotation == -180.0f) hOrbitSnapInput = 0.0f;
        }
        else
        {
            if (yOrbitSnapInput > 0)
            {
                ResetLookOnTargetYOrbit();
            }
        }

        orbit.xRotation += -vOrbitInput * orbit.vOrbitSmooth * Time.deltaTime;
        orbit.yRotation += hOrbitInput * orbit.hOrbitSmooth * Time.deltaTime;

        if (orbit.xRotation > orbit.maxXRotation)
        {
            orbit.xRotation = orbit.maxXRotation;
        }

        if (orbit.xRotation < orbit.minXRotation)
        {
            orbit.xRotation = orbit.minXRotation;
        }

        if (orbit.yRotation > 360f) orbit.yRotation = orbit.yRotation - 360f;
        if (orbit.yRotation < 0) orbit.yRotation = 360f - orbit.yRotation;
    }

    void ZoomOnTarget()
    {
        position.distanceFromTarget += zoomInput * position.zoomSmooth * Time.deltaTime;

        if (position.distanceFromTarget > position.maxZoom)
        {
            position.distanceFromTarget = position.maxZoom;
        }

        if (position.distanceFromTarget < position.minZoom)
        {
            position.distanceFromTarget = position.minZoom;
        }

        zoomInput = 0.0f;
    }

    void ResetLookOnTarget()
    {
        ResetLookOnTargetYOrbit();
        orbit.xRotation = -10.0f;
    }

    void ResetLookOnTargetYOrbit()
    {
        orbit.yRotation = -180.0f;
    }

    private void CheckCollisions()
    {
        collision.UpdateCameraClipPoints(transform.position, transform.rotation, ref collision.adjustedCameraClipPoints);
        collision.UpdateCameraClipPoints(destination, transform.rotation, ref collision.desiredCameraClipPoints);

        // draw debug lines
        for (int i = 0; i < 5; i++)
        {
            if (debug.drawAdjustedCollisionLines)
            {
                Debug.DrawLine(targetPos, collision.desiredCameraClipPoints[i], Color.white);
            }
            if (debug.drawDesiredCollisionLines)
            {
                Debug.DrawLine(targetPos, collision.adjustedCameraClipPoints[i], Color.green);
            }
        }

        collision.CheckCollision(targetPos);
        position.adjustmentDistance = collision.GetAdjustedDistanceWithRayFrom(targetPos);
    }

    public float Get_VOrbitSmooth()
    {
        return orbit.vOrbitSmooth;
    }

    public void SetCameraTarget(Transform newTarget)
    {
        target = newTarget;

        if (target == null)
        {
            Debug.LogError("There's no target.");
            return;
        }

        if (onChangeTarget != null)
        {
            onChangeTarget();
        }
    }

    // ### Control Functions
    public void SetOrbitAxis(float verticalOrbitAxis = 0.0f, float horizontalOrbitAxis = 0.0f)
    {
        if (!canControlCamera) return;

        vOrbitInput = verticalOrbitAxis;
        hOrbitInput = horizontalOrbitAxis;
    }

    public void SetZoomIn()
    {
        zoomInput = position.zoomSpeed;
    }

    public void SetZoomOut()
    {
        zoomInput = -position.zoomSpeed;
    }

    public void SetSnapOrbit()
    {
        hOrbitSnapInput = 1.0f;
        //hOrbitSnapInput = 0.0f;
    }

    public void SetSnapYOrbit(float _value = 1.0f)
    {
        yOrbitSnapInput = _value;
    }

    public void SetControlCamera(bool _toControl)
    {
        canControlCamera = _toControl;
    }

    public void SetHOrbitInput(float _horbitInput)
    {
        hOrbitInput = _horbitInput;
    }

    public bool CanControlCamera()
    {
        return canControlCamera;
    }

    public bool IsOnBackOfTarget()
    {
        return orbit.yRotation == orbit.initialYRotation;
    }

    [Serializable]
    public class CollisionHandler
    {
        public LayerMask collisionLayerMask;

        public float collisionSize = 3.41f;

        [HideInInspector] public bool isColliding = false;
        [HideInInspector] public Vector3[] adjustedCameraClipPoints;
        [HideInInspector] public Vector3[] desiredCameraClipPoints;

        Camera camera;

        public void Initialize(Camera _camera)
        {
            camera = _camera;
            adjustedCameraClipPoints = new Vector3[5];
            desiredCameraClipPoints = new Vector3[5];
        }

        public void UpdateCameraClipPoints(Vector3 cameraPosition, Quaternion atRotation, ref Vector3[] intoArray)
        {
            if (!camera) return;

            // Clear the contents of intoArray
            intoArray = new Vector3[5];

            float z = camera.nearClipPlane;
            float x = Mathf.Tan(camera.fieldOfView / collisionSize) * z;
            float y = x / camera.aspect;

            // Top Left
            intoArray[0] = (atRotation * new Vector3(-x, y, z) + cameraPosition); // Added and rotated the point relative to camera

            // Top Right
            intoArray[1] = (atRotation * new Vector3(x, y, z) + cameraPosition); // Added and rotated the point relative to camera

            // Bottom Left
            intoArray[2] = (atRotation * new Vector3(-x, -y, z) + cameraPosition); // Added and rotated the point relative to camera

            // Bottom Right
            intoArray[3] = (atRotation * new Vector3(x, -y, z) + cameraPosition); // Added and rotated the point relative to camera

            // Camera's position
            intoArray[4] = cameraPosition - camera.transform.forward;
        }

        public void CheckCollision(Vector3 targetPosition)
        {
            isColliding = CollisionDetectedAtClipPoints(desiredCameraClipPoints, targetPosition);
        }

        bool CollisionDetectedAtClipPoints(Vector3[] clipPoints, Vector3 fromPosition)
        {
            for (int i = 0; i < clipPoints.Length; i++)
            {
                Ray ray = new Ray(fromPosition, clipPoints[i] - fromPosition);
                float distance = Vector3.Distance(clipPoints[i], fromPosition);
                if (Physics.Raycast(ray, distance, collisionLayerMask))
                {
                    return true;
                }
            }

            return false;
        }

        public float GetAdjustedDistanceWithRayFrom(Vector3 fromPosition)
        {
            float distance = -1.0f;

            for (int i = 0; i < desiredCameraClipPoints.Length; i++)
            {
                Ray ray = new Ray(fromPosition, desiredCameraClipPoints[i] - fromPosition);
                RaycastHit hit;

                /*if (Physics.Raycast(ray, out hit))
                {
                    if (distance == -1.0f || hit.distance < distance)
                    {
                        distance = hit.distance;
                    }
                }*/

                if (Physics.Raycast(ray, out hit))
                {
                    if (distance == -1f)
                    {
                        distance = hit.distance;
                    }
                    else
                    {
                        if (hit.distance < distance) distance = hit.distance;
                    }
                }
            }

            if (distance == -1.0f)
            {
                return 0.0f;
            }
            else
            {
                return distance;
            }
        }
    }
}
