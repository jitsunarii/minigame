using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class TestUni : MonoBehaviour
{
    void Start()
    {
        moveCube();


}
private async UniTask moveCube()
    {
        transform.position = new Vector3(0, 0, 0);
        await UniTask.Delay(3000);
        transform.position = new Vector3(1f, 0, 0);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
