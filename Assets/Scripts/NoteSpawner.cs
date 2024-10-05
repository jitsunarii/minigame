using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;

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
            XRGrabInteractable grabInteractable = note.GetComponent<XRGrabInteractable>();
            if (grabInteractable != null)
            {
                grabInteractable.selectEntered.AddListener(ballController.OnSelectEntered);
            }
        }
    }
}