using System;
using UnityEngine;

public class BeatObject : MonoBehaviour
{
    public float velocityThreshold = 0.3f;
    public float maxColorIntensity = 2.0f;
    public float colorIntensityOffset = 0.5f;
    [NonSerialized] public float collisionVelocityMagnitude;

    private Renderer objectRenderer;
    private Color originalColor;
    private Color targetColor = Color.red;

    public event Action BeatEvent;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }
        else
        {
            Debug.LogWarning("Renderer component not found on " + gameObject.name);
        }
    }

    public void ChangeColorBasedOnVelocity()
    {
        if (objectRenderer == null) return;

        float intensity = CalculateColorIntensity();
        Color newColor = Color.Lerp(originalColor, targetColor, intensity);
        objectRenderer.material.color = newColor;

        BeatEvent?.Invoke();
    }

    private float CalculateColorIntensity()
    {
        float intensity = collisionVelocityMagnitude / maxColorIntensity + colorIntensityOffset;
        return Mathf.Clamp01(intensity);
    }

    public void ResetColor()
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.color = originalColor;
        }
    }
}
