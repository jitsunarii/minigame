using UnityEngine;
using System.Collections;

public class Beat : MonoBehaviour
{
    [SerializeField] private float timeThread = 0.1f;
    [SerializeField] private RaysManager raysManager;
    private float velocityThreshold = 0.6f; // この変数名を変更
    private float lastCollisionTime = 0;
    private float maxVelocityMagnitude;
    private string collisionName;
    private BeatObject beatObject;

    private void Start()
    {
        ResetCollisionObj();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Time.time - lastCollisionTime < timeThread)
        {
            return;
        }

        beatObject = collision.transform.GetComponent<BeatObject>();
        if (beatObject == null)
        {
            return;
        }

        velocityThreshold = beatObject.velocityThreshold; // ここを修正
        collisionName = beatObject.name;

        maxVelocityMagnitude = raysManager.GetMinAverageVelocity(collisionName);
        beatObject.collisionVelocityMagnitude = maxVelocityMagnitude;

        if (maxVelocityMagnitude >= velocityThreshold)
        {
            beatObject.ChangeColorBasedOnVelocity();

            lastCollisionTime = Time.time;

            StartCoroutine(ResetColorAfterDelay(beatObject));

            ResetCollisionObj();
        }
    }

    private IEnumerator ResetColorAfterDelay(BeatObject obj)
    {
        yield return new WaitForSeconds(0.5f);
        obj.ResetColor();
    }

    private void ResetCollisionObj()
    {
        maxVelocityMagnitude = 0;
        collisionName = string.Empty;
    }
}