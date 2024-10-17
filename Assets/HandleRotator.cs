using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleRotator : MonoBehaviour
{
    public Transform cylinderTransform; // Cylinderのトランスフォーム
    public float rotationSpeed = 5f; // 回転速度

    private Vector3 lastMousePosition;
    private bool isDragging = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // マウスの左ボタンが押された時
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform == this.transform)
            {
                isDragging = true;
                lastMousePosition = Input.mousePosition;
            }
        }
        else if (Input.GetMouseButtonUp(0)) // マウスの左ボタンが離された時
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 deltaMouse = Input.mousePosition - lastMousePosition;
            float rotationAmount = deltaMouse.x * rotationSpeed * Time.deltaTime;

            // Cylinderを回転
            cylinderTransform.Rotate(Vector3.up, rotationAmount, Space.World);

            lastMousePosition = Input.mousePosition;
        }
    }
}
