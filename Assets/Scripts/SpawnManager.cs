using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject enemyPrefab;

    void Start()
    {
        
        StartCoroutine(SpawnRoutine());
    }

    void RandomSpawn()
    {
        var index = Random.Range(0, spawnPoints.Length);
        var spawnPoint = spawnPoints[index];
        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(5);
        while (true)
        {
            RandomSpawn();
            yield return new WaitForSeconds(3);
        }
    }
}
