using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaysManager : MonoBehaviour
{
    [SerializeField] private HandRay[] handRays;

    // オブジェクトに触れているRayのうち、最も短いものの相対速度を取得する
    public float GetMinAverageVelocity(string name)
    {
        float minDistance = 1;
        float averageVelocity = 0;
        HandRay minDistanceHandRay = null;

        foreach(HandRay handRay in handRays)
        {
            if(handRay.hitObjectTrans == null)
            {
                continue;
            }

            // Rayが指定のオブジェクトに触れている場合
            if(handRay.hitObjectTrans.name == name)
            {
                // 最短距離のRayを取得
                float distance = handRay.oldDistance;
                if(distance < minDistance)
                {
                    minDistance = distance;
                    minDistanceHandRay = handRay;
                }
            }
        }

        // Rayを取得した場合
        if (minDistanceHandRay != null)
        {
            // そのRayの平均距離を取得
            averageVelocity = minDistanceHandRay.GetAverageVelocity();
        }

        return averageVelocity;
    }
}
