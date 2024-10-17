using UnityEngine;
using DG.Tweening;
using UnityEngine.XR.Interaction.Toolkit;

public class BallController : MonoBehaviour
{
    public void Initialize()
    {
        transform.DOLocalPath(
            new[]
            {
                new Vector3(2.1f, 2.0f, 1.5f),
                new Vector3(0f, 1.4f, -3f),
            },
            3f,
            PathType.CatmullRom)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => DestroyBall());
    }

    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        DestroyBall();
        GameDirector.Instance.AddScore(10);

    }

    private void DestroyBall()
    {
        Destroy(gameObject);
    }
}