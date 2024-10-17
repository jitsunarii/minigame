using UnityEngine;

public class HandleGrabRotator : MonoBehaviour
{
    public Transform handleTransform;   // 新しく追加した Handle オブジェクト
    public Transform capsuleTransform;
    public Camera mainCamera;
    public float rotationSpeed = 5f;
    public float maxZDistance = 1f;
    public Color grabColor = Color.yellow;

    private Vector3 initialLocalPosition;
    private Plane movementPlane;
    private bool isGrabbing = false;
    private Vector3 grabOffset;
    private Renderer capsuleRenderer;
    private Color originalColor;

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

        initialLocalPosition = capsuleTransform.localPosition;
        movementPlane = new Plane(mainCamera.transform.right, handleTransform.position);
        
        capsuleRenderer = capsuleTransform.GetComponent<Renderer>();
        if (capsuleRenderer != null)
        {
            originalColor = capsuleRenderer.material.color;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform == capsuleTransform)
            {
                isGrabbing = true;
                grabOffset = capsuleTransform.InverseTransformPoint(hit.point);
                SetCapsuleColor(grabColor);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isGrabbing = false;
            SetCapsuleColor(originalColor);
        }

        if (isGrabbing)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            float enter;

            if (movementPlane.Raycast(ray, out enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                Vector3 targetPosition = hitPoint - handleTransform.TransformDirection(grabOffset);
                Vector3 localTargetPosition = handleTransform.InverseTransformPoint(targetPosition);

                // z座標のみを使用し、範囲を制限
                float newLocalZ = Mathf.Clamp(localTargetPosition.z, -maxZDistance, maxZDistance);

                // Capsuleの新しい位置を設定（x と y は初期位置を維持）
                Vector3 newLocalPosition = new Vector3(initialLocalPosition.x, initialLocalPosition.y, newLocalZ);
                capsuleTransform.localPosition = newLocalPosition;

                // Handleの回転を計算
                float rotationAngle = (newLocalZ / maxZDistance) * 180f;
                Quaternion targetRotation = Quaternion.Euler(0, rotationAngle, 0);
                handleTransform.localRotation = Quaternion.Slerp(handleTransform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            // Capsuleを元の位置に戻す
            capsuleTransform.localPosition = Vector3.Lerp(capsuleTransform.localPosition, initialLocalPosition, rotationSpeed * Time.deltaTime);
            
            // Handleを元の回転に戻す
            handleTransform.localRotation = Quaternion.Slerp(handleTransform.localRotation, Quaternion.identity, rotationSpeed * Time.deltaTime);
        }
    }

    void SetCapsuleColor(Color color)
    {
        if (capsuleRenderer != null)
        {
            capsuleRenderer.material.color = color;
        }
    }
}