using UnityEngine;

public class CapsuleHitRotator : MonoBehaviour
{
    public Transform handleTransform;
    public Transform capsuleTransform;
    public Camera mainCamera;
    public float forceMagnitude = 500f;
    public float maxAngularVelocity = 10f;
    public Color grabColor = Color.yellow;
    public LayerMask raycastLayerMask = -1;

    private Rigidbody handleRigidbody;
    private bool isGrabbing = false;
    private Vector3 lastMousePosition;
    private Renderer capsuleRenderer;
    private Color originalColor;
    private CapsuleCollider capsuleCollider;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (handleTransform == null || capsuleTransform == null)
        {
            Debug.LogError("Please assign Handle and Capsule transforms in the inspector.");
            enabled = false;
            return;
        }

        handleRigidbody = handleTransform.GetComponent<Rigidbody>();
        if (handleRigidbody == null)
        {
            handleRigidbody = handleTransform.gameObject.AddComponent<Rigidbody>();
        }
        handleRigidbody.useGravity = false;
        handleRigidbody.maxAngularVelocity = maxAngularVelocity;

        capsuleRenderer = capsuleTransform.GetComponent<Renderer>();
        if (capsuleRenderer != null)
        {
            originalColor = capsuleRenderer.material.color;
        }
        else
        {
            Debug.LogWarning("Capsule renderer not found. Color change will not work.");
        }

        capsuleCollider = capsuleTransform.GetComponent<CapsuleCollider>();
        if (capsuleCollider == null)
        {
            Debug.LogError("Capsule does not have a CapsuleCollider. Adding one.");
            capsuleCollider = capsuleTransform.gameObject.AddComponent<CapsuleCollider>();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1f);

            Vector3 point1, point2;
            float radius;
            GetCapsulePoints(capsuleCollider, out point1, out point2, out radius);

            RaycastHit hit;
            if (Physics.CapsuleCast(point1, point2, radius, ray.direction, out hit, Mathf.Infinity, raycastLayerMask))
            {
                Debug.Log($"Hit object: {hit.transform.name}");
                if (hit.transform == capsuleTransform)
                {
                    isGrabbing = true;
                    lastMousePosition = Input.mousePosition;
                    SetCapsuleColor(grabColor);
                    Debug.Log("Grabbing started");
                }
            }
            else
            {
                Debug.Log("No object hit by capsule cast");
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isGrabbing = false;
            SetCapsuleColor(originalColor);
            Debug.Log("Grabbing ended");
        }

        if (isGrabbing)
        {
            Vector3 deltaMouse = Input.mousePosition - lastMousePosition;
            Vector3 forceDirection = mainCamera.transform.right * deltaMouse.x + mainCamera.transform.up * deltaMouse.y;
            forceDirection = Vector3.ProjectOnPlane(forceDirection, handleTransform.up).normalized;

            handleRigidbody.AddTorque(handleTransform.up * Vector3.Dot(forceDirection, handleTransform.forward) * forceMagnitude * Time.deltaTime);

            lastMousePosition = Input.mousePosition;
        }
    }

    void SetCapsuleColor(Color color)
    {
        if (capsuleRenderer != null)
        {
            capsuleRenderer.material.color = color;
        }
    }

    void GetCapsulePoints(CapsuleCollider capsule, out Vector3 point1, out Vector3 point2, out float radius)
    {
        Vector3 center = capsule.transform.TransformPoint(capsule.center);
        radius = capsule.radius;
        float height = capsule.height;
        int direction = capsule.direction;

        Vector3 axisDirection = direction == 0 ? capsule.transform.right :
                                direction == 1 ? capsule.transform.up :
                                                 capsule.transform.forward;

        float halfHeight = height / 2f - radius;
        point1 = center + axisDirection * halfHeight;
        point2 = center - axisDirection * halfHeight;
    }

    void OnDrawGizmos()
    {
        if (capsuleCollider != null)
        {
            Gizmos.color = Color.green;
            Vector3 point1, point2;
            float radius;
            GetCapsulePoints(capsuleCollider, out point1, out point2, out radius);
            Gizmos.DrawWireSphere(point1, radius);
            Gizmos.DrawWireSphere(point2, radius);
            Gizmos.DrawLine(point1, point2);
        }
    }
}
