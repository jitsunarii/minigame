using UnityEngine;

public class BallMover : MonoBehaviour
{
    public AnimationCurve xCurve = AnimationCurve.Linear(0, 0, 1, 0);
    public AnimationCurve yCurve = AnimationCurve.Linear(0, 0, 1, 0);
    public AnimationCurve zCurve = AnimationCurve.Linear(0, 0, 1, 1);
    
    public float moveSpeed = 1f;
    public float totalDistance = 10f;
    
    public Vector3 startPosition;
    public Vector3 endPosition;

    private float startTime;

    private void Start()
    {
        startTime = Time.time;
        transform.position = startPosition;
    }

    private void Update()
    {
        float t = (Time.time - startTime) * moveSpeed / totalDistance;

        if (t > 1f)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 newPosition = new Vector3(
            Mathf.Lerp(startPosition.x, endPosition.x, xCurve.Evaluate(t)),
            Mathf.Lerp(startPosition.y, endPosition.y, yCurve.Evaluate(t)),
            Mathf.Lerp(startPosition.z, endPosition.z, zCurve.Evaluate(t))
        );

        transform.position = newPosition;
    }

    public void Initialize(Vector3 start, Vector3 end, float speed)
    {
        startPosition = start;
        endPosition = end;
        moveSpeed = speed;
        totalDistance = Vector3.Distance(start, end);
        transform.position = startPosition;
        startTime = Time.time;
    }
}
