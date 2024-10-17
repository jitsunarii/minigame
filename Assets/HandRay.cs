using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandRay : MonoBehaviour
{
    
    public Transform hitObjectTrans { get; private set; }
    public float oldDistance { get; private set; }

    private float maxRayLength = 0.2f;

    private int bufferNumber = 5;
    private float[] velocityBuffer;
    private int bufferIndex = 0;
    private bool isHit = false;

    // Start is called before the first frame update
    void Start()
    {
        ResetBuffer();
        hitObjectTrans = null;
    }

    // Update is called once per frame
    void Update()
    {
        // UpdateでRayを飛ばし、そのRayが当たったオブジェクトとの距離を測り、
        // その距離の変化量（＝相対速度）を計算してリングバッファに格納する

        // Rayを前方にmaxRayLengthの長さ飛ばす
        RaycastHit hit;

        // 視覚的にRayを表示する
        Debug.DrawRay(transform.position, transform.forward * maxRayLength, Color.red, 0.1f);

        // Rayが何かに当たった場合
        if(Physics.Raycast(transform.position, transform.forward, out hit, maxRayLength))
        {
            // Rayが初めてオブジェクトに当たった
            if(hit.transform != hitObjectTrans)
            {
                oldDistance = hit.distance;
                hitObjectTrans = hit.transform;
                isHit = true;
            }
            // ずっと同じオブジェクトにRayが当たっている
            else
            {
                // 相対速度を計算し、バッファに格納
                float velocity = (oldDistance - hit.distance) / Time.deltaTime;
                velocityBuffer[bufferIndex] = velocity;
                bufferIndex = (bufferIndex + 1) % bufferNumber;

                oldDistance = hit.distance;
            }
        }
        // Rayが当たっていない場合
        else
        {
            // Rayがこれまで当たっていたオブジェクトから外れた場合
            if (isHit)
            {
                // バッファをリセット
                ResetBuffer();
            }
        }
    }

    // バッファのリセット
    private void ResetBuffer()
    {
        velocityBuffer = new float[bufferNumber];
        bufferIndex = 0;
        hitObjectTrans = null;
        isHit = false;
    }

    // 平均の相対速度を取得
    public float GetAverageVelocity()
    {
        // バッファに格納されている速度の平均を計算する

        float averageVelocity = 0;

        foreach(float v in velocityBuffer)
        {
            averageVelocity += v;
        }

        averageVelocity /= bufferNumber;

        return averageVelocity;
    }
}
