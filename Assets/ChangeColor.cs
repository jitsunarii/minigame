using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    private Renderer renderer;

    void Start()
    {
        renderer = this.gameObject.GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Hand_Index3_CapsuleCollider")
        {
            renderer.material.color = Color.red;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.name == "Hand_Index3_CapsuleCollider")
        {
            renderer.material.color = Color.white;
        }
    }
}
