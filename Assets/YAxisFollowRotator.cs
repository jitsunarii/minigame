using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ZAxisFollowRotator : MonoBehaviour
{
    public Transform cylinderTransform;  // Cylinderのトランスフォーム
    public Transform capsuleTransform;   // Capsuleのトランスフォーム
    public Camera mainCamera;
    public float rotationSpeed = 5f;
    public float maxZDistance = 1f;      // Capsuleの前後方向の最大移動距離

    private Vector3 initialLocalPosition;
    private float cylinderRadius;
    private Plane movementPlane;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (cylinderTransform == null || capsuleTransform == null)
        {
            Debug.LogError("Please assign Cylinder and Capsule transforms in the inspector.");
            enabled = false;
            return;
        }

        initialLocalPosition = capsuleTransform.localPosition;
        cylinderRadius = cylinderTransform.localScale.x * 0.5f;
        movementPlane = new Plane(mainCamera.transform.right, cylinderTransform.position);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))  // マウスボタンが押されている間
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            float enter;

            if (movementPlane.Raycast(ray, out enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                Vector3 localHitPoint = cylinderTransform.InverseTransformPoint(hitPoint);

                // z座標のみを使用し、範囲を制限
                float newLocalZ = Mathf.Clamp(localHitPoint.z, -maxZDistance, maxZDistance);

                // Capsuleの新しい位置を設定（x と y は初期位置を維持）
                Vector3 newLocalPosition = new Vector3(initialLocalPosition.x, initialLocalPosition.y, newLocalZ);
                capsuleTransform.localPosition = newLocalPosition;

                // Cylinderの回転を計算
                float rotationAngle = (newLocalZ / maxZDistance) * 180f; // -180度から180度の範囲で回転
                Quaternion targetRotation = Quaternion.Euler(0, rotationAngle, 0);
                cylinderTransform.localRotation = Quaternion.Slerp(cylinderTransform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        else  // マウスボタンが押されていない時
        {
            // Capsuleを元の位置に戻す
            capsuleTransform.localPosition = Vector3.Lerp(capsuleTransform.localPosition, initialLocalPosition, rotationSpeed * Time.deltaTime);
            
            // Cylinderを元の回転に戻す
            cylinderTransform.localRotation = Quaternion.Slerp(cylinderTransform.localRotation, Quaternion.identity, rotationSpeed * Time.deltaTime);
        }
    }
}