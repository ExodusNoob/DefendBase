using System.Collections;
using System.Reflection;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float margin = 0.5f;

    [Header("SpawnSettings")]
    [SerializeField] float spawnDelay = 1f;
    [SerializeField] int waveTrigger = 20;
    [SerializeField] int waveSize = 8;

    private BoxCollider2D roomCollider;

    [SerializeField]
    int totalEnemies;
    [SerializeField]
    int spawnedEnemies;

    private void Awake()
    {
        roomCollider = GetComponent<BoxCollider2D>();
    }

    public void SpawnEnemies()
    {
        Debug.Log("recibimos que los enemigos pueden tener el nivel " + GameManager.Instance.SelectedEnemyLevel);

        totalEnemies = EnemyManager.Instance.GetEnemyAmount();
        spawnedEnemies = 0;

        StartCoroutine(SpawnRoutine());

        //int amount = EnemyManager.Instance.GetEnemyAmount();
        //
        //for (int i = 0; i < amount; i++)
        //{
        //    GameObject enemy = EnemyManager.Instance.GetRandomEnemy();
        //
        //    Vector3 pos = GetSpawnPosition();
        //
        //    Instantiate(enemy, pos, Quaternion.identity);
        //}
    }

    IEnumerator SpawnRoutine()
    {
        while (spawnedEnemies < totalEnemies)
        {
            SpawnEnemy();

            spawnedEnemies++;

            if (spawnedEnemies % waveTrigger == 0)
            {
                StartCoroutine(BigWave());
            }
            yield return new WaitForSeconds(spawnDelay);
        }
    }
    IEnumerator BigWave()
    {
        Debug.Log("Gran Ola waaaaaaa");

        for (int i = 0; i < waveSize; i++)
        {
            if (spawnedEnemies >= totalEnemies)
                yield break;

            SpawnEnemy();

            spawnedEnemies++;

            yield return new WaitForSeconds(0.2f);
        }
    }
    void SpawnEnemy()
    {
        GameObject enemy = EnemyManager.Instance.GetRandomEnemy();

        Vector3 pos = GetSpawnPosition();

        Instantiate(enemy, pos, Quaternion.identity);
    }

    Vector3 GetSpawnPosition()
    {
        Bounds bounds = roomCollider.bounds;

        float randomX = Random.Range(bounds.min.x + margin, bounds.max.x - margin);
        float randomY = Random.Range(bounds.min.y + margin, bounds.max.y - margin);

        return new Vector3(randomX, randomY, 0f);
    }
}