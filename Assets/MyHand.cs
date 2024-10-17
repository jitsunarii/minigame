using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyHand : MonoBehaviour
{    
    private OVRHand hand;
    private Renderer renderer;

    void Start()
    {
        renderer = GameObject.FindWithTag("cube").GetComponent<Renderer>();
        Debug.Log(renderer);
        hand = this.gameObject.GetComponent<OVRHand>();
    }

    void Update()
    {
        if (hand.GetFingerIsPinching(OVRHand.HandFinger.Middle))
        {
            renderer.material.color = Color.blue;
        }
        else if (renderer.material.color == Color.blue)
        {
            renderer.material.color = Color.white;
        }
    }
}