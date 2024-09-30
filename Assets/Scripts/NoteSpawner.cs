using UnityEngine;
using System.Collections;

public class NoteSpawner : MonoBehaviour
{
    public GameObject notePrefab;
    public float spawnInterval = 1f;
    public Vector3 spawnPosition = new Vector3(0f, 0.8f, 5f);

    private Coroutine spawnCoroutine;

    public void StartSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
        spawnCoroutine = StartCoroutine(SpawnNotes());
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    private IEnumerator SpawnNotes()
    {
        while (true)
        {
            SpawnNote();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnNote()
    {
        GameObject note = Instantiate(notePrefab, spawnPosition, Quaternion.identity);
        BallController ballController = note.GetComponent<BallController>();
        if (ballController != null)
        {
            ballController.Initialize();
        }
    }
}