using UnityEngine;  // UnityEngineの名前空間を使用
using System.Collections;  // IEnumeratorを使用するために必要

public class NoteSpawner : MonoBehaviour  // MonoBehaviourを継承したNoteSpawnerクラスを定義
{
    public GameObject notePrefab;  // インスペクターでアサインする、生成するノートのプレハブ
    public float spawnInterval = 1f;  // ノートを生成する間隔（秒）
    public Vector3 spawnPosition = new Vector3(0f, 0.8f, 5f);  // ノートを生成する位置

    private void Start()  // スクリプトが開始されたときに呼ばれるメソッド
    {
        StartCoroutine(SpawnNotes());  // ノート生成のコルーチンを開始
    }

    private IEnumerator SpawnNotes()  // ノートを定期的に生成するコルーチン
    {
        while (true)  // 無限ループ（ゲームが続く限りノートを生成し続ける）
        {
            SpawnNote();  // ノートを1つ生成
            yield return new WaitForSeconds(spawnInterval);  // 指定した間隔だけ待機
        }
    }

    private void SpawnNote()  // 実際にノートを生成するメソッド
    {
        GameObject note = Instantiate(notePrefab, spawnPosition, Quaternion.identity);
        // プレハブからノートオブジェクトを生成し、指定位置に配置
        BallController ballController = note.GetComponent<BallController>();
        // 生成したノートからBallControllerコンポーネントを取得
        if (ballController != null)  // BallControllerコンポーネントが存在する場合
        {
            ballController.Initialize();  // BallControllerの初期化メソッドを呼び出し
        }
    }
}