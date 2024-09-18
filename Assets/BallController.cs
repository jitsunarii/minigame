using UnityEngine;
using DG.Tweening;

public class BallController : MonoBehaviour
{
    public void Initialize()
    {
        transform.DOLocalPath(
            new[]
            {
                new Vector3(2.1f, 0.8f, 1.5f),
                new Vector3(0f, 0.8f, -2.5f),
            },
            3f,
            PathType.CatmullRom)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => DestroyBall());
    }

    private void OnMouseDown()
    {
        DestroyBall();
    }

    private void DestroyBall()
    {
        GameDirector.Instance.AddScore(10);
        Destroy(gameObject);
    }
}