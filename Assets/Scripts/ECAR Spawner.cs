using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ECARSpawner : MonoBehaviour
{
    public List<GameObject> prefabs;
    public float fixedYPosition = 0f;
    public float minX = -10f;
    public float maxX = 10f;
    public float spawnInterval = 2f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(SpawnObject), spawnInterval, spawnInterval);
    }

    void SpawnObject()
    {
        int index = Random.Range(0, prefabs.Count);
        GameObject selectedPrefab = prefabs[index];

        GameObject instance = Instantiate(selectedPrefab);
        float randomX = Random.Range(minX, maxX);
        instance.transform.position = new Vector3(randomX, fixedYPosition, 0);
    }
}
