using UnityEngine;
using DG.Tweening;
using UnityEngine.XR.Interaction.Toolkit;

public class BallController : MonoBehaviour
{
private XRGrabInteractable grabInteractable;
    private void Awake()
    {
        grabInteractable = gameObject.AddComponent<XRGrabInteractable>();
        grabInteractable.movementType = XRBaseInteractable.MovementType.Kinematic;
        grabInteractable.throwOnDetach = false;
    }
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

    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        DestroyBall();
    }

    private void DestroyBall()
    {
        GameDirector.Instance.AddScore(10);
        Destroy(gameObject);
    }
}


// using UnityEngine;
// using DG.Tweening;

// public class BallController : MonoBehaviour
// {
//     public void Initialize()
//     {
//         transform.DOLocalPath(
//             new[]
//             {
//                 new Vector3(2.1f, 0.8f, 1.5f),
//                 new Vector3(0f, 0.8f, -2.5f),
//             },
//             3f,
//             PathType.CatmullRom)
//             .SetEase(Ease.InOutSine)
//             .OnComplete(() => DestroyBall());
//     }

//     private void OnMouseDown()
//     {
//         DestroyBall();
//     }

//     private void DestroyBall()
//     {
//         GameDirector.Instance.AddScore(10);
//         Destroy(gameObject);
//     }
// }