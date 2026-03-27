using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class WaveConfig
    {
        public int totalSpawnEnemies;
        public int numberOfRandomSpawnPoint;
        public float delayStart;
        public float spawnInterval;
        public int numberOfPowerUp;
    }

    public List<WaveConfig> waves;

    public Transform[] allSpawnPoints;
    public GameObject enemyPrefab;
    public GameObject[] powerUpPrefab;

    private int currentWave = 0;

    void Start()
    {
        Debug.Log("🚀 Game Start - Begin Wave System");
        StartCoroutine(SpawnWaveRoutine());
    }

    IEnumerator SpawnWaveRoutine()
    {
        while (currentWave < waves.Count)
        {
            WaveConfig wave = waves[currentWave];

            Debug.Log($"🌊 Wave {currentWave + 1} START");

            // สุ่ม spawn point
            List<Transform> selectedPoints = GetRandomSpawnPoints(wave.numberOfRandomSpawnPoint);
            Debug.Log($"📍 Selected SpawnPoints: {selectedPoints.Count}");

            // spawn powerup
            SpawnPowerUps(wave.numberOfPowerUp);

            // delay ก่อนเริ่ม
            Debug.Log($"⏳ Waiting {wave.delayStart} sec before spawning enemies");
            yield return new WaitForSeconds(wave.delayStart);

            // spawn enemy
            for (int i = 0; i < wave.totalSpawnEnemies; i++)
            {
                Transform point = selectedPoints[Random.Range(0, selectedPoints.Count)];
                Instantiate(enemyPrefab, point.position, Quaternion.identity);

                Debug.Log($"👾 Spawn Enemy {i + 1}/{wave.totalSpawnEnemies}");

                yield return new WaitForSeconds(wave.spawnInterval);
            }

            Debug.Log($"✅ Wave {currentWave + 1} FINISHED");

            currentWave++;
        }

        Debug.Log("🏁 ALL WAVES COMPLETED");
    }

    List<Transform> GetRandomSpawnPoints(int count)
    {
        List<Transform> list = new List<Transform>(allSpawnPoints);

        for (int i = 0; i < list.Count; i++)
        {
            Transform temp = list[i];
            int rand = Random.Range(i, list.Count);
            list[i] = list[rand];
            list[rand] = temp;
        }

        return list.GetRange(0, count);
    }

    void SpawnPowerUps(int count)
    {
        int spawned = 0;

        // ใช้ spawn point เป็นฐาน
        List<Transform> availablePoints = new List<Transform>(allSpawnPoints);

        // shuffle
        for (int i = 0; i < availablePoints.Count; i++)
        {
            int rand = Random.Range(i, availablePoints.Count);
            var temp = availablePoints[i];
            availablePoints[i] = availablePoints[rand];
            availablePoints[rand] = temp;
        }

        Debug.Log($"⚡ [PowerUp] ต้องการ spawn = {count}, จุดทั้งหมด = {availablePoints.Count}");

        foreach (Transform point in availablePoints)
        {
            if (spawned >= count)
                break;

            Vector3 spawnPos = point.position + Vector3.up;

            // 🔍 เช็คด้วย OverlapSphere
            Collider[] hits = Physics.OverlapSphere(spawnPos, 1.0f);

            bool hasPowerUp = false;

            foreach (var hit in hits)
            {
                if (hit.CompareTag("PowerUp"))
                {
                    hasPowerUp = true;
                    Debug.Log($"❌ [PowerUp] จุด {point.name} มีของอยู่แล้ว");
                    break;
                }
            }

            if (hasPowerUp)
                continue;

            // ✅ spawn
            int puIndex = Random.Range(0, powerUpPrefab.Length);
            GameObject pu = Instantiate(powerUpPrefab[puIndex], spawnPos, Quaternion.identity);

            spawned++;

            Debug.Log($"✅ [PowerUp] Spawn {powerUpPrefab[puIndex].name} at {point.name}");
        }

        // 🔥 spawn ไม่ครบ
        if (spawned < count)
        {
            int failed = count - spawned;
            Debug.LogWarning($"⚠️ [PowerUp] Spawn ไม่ครบ ขาด {failed} (พื้นที่เต็ม / จุดไม่พอ)");
        }
        else
        {
            Debug.Log("🎯 [PowerUp] Spawn ครบทุกชิ้น");
        }
    }
}