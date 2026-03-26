using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> allEnemies;

    public static EnemyManager Instance;

    public int currentLevel = 1;

    [System.Serializable]
    public class EnemyEntry
    {
        public GameObject enemyPrefab;
        [Range(0, 100)] public float spawnChance;
    }

    [System.Serializable]
    public class LevelEnemyConfig
    {
        public int level;

        public int minEnemies;
        public int maxEnemies;

        public List<EnemyEntry> enemies;
    }

    public List<LevelEnemyConfig> levelConfigs;

    private void Awake()
    {
        Instance = this;
        currentLevel = GameManager.Instance.SelectedEnemyLevel;
    }

    LevelEnemyConfig GetCurrentLevel()
    {
        LevelEnemyConfig config = levelConfigs.Find(l => l.level == currentLevel);

        if (config != null)
            return config;

        // Si no existe, generar uno procedural
        return GenerateProceduralLevel(currentLevel);
    }

    public int GetEnemyAmount()
    {
        LevelEnemyConfig config = GetCurrentLevel();

        if (config == null)
        {
            Debug.LogWarning("No hay config para este nivel");
            return 0;
        }

        return Random.Range(config.minEnemies, config.maxEnemies + 1);
    }

    public GameObject GetRandomEnemy()
    {
        LevelEnemyConfig config = GetCurrentLevel();

        if (config == null)
        {
            Debug.LogWarning("No hay config para este nivel");
            return null;
        }

        float randomValue = Random.Range(0f, 100f);
        float cumulative = 0;

        foreach (var enemy in config.enemies)
        {
            cumulative += enemy.spawnChance;

            if (randomValue <= cumulative)
                return enemy.enemyPrefab;
        }

        return config.enemies[0].enemyPrefab;
    }
    LevelEnemyConfig GenerateProceduralLevel(int level)
    {
        LevelEnemyConfig newConfig = new LevelEnemyConfig();

        newConfig.level = level;

        newConfig.minEnemies = Mathf.Clamp(level / 2, 5, 500); // valor nivel/2 , min y maximo de enemigos
        newConfig.maxEnemies = Mathf.Clamp(level, 10, 1000); //maximo de enemigos, segun el nivel, entre 10 y 1k

        newConfig.enemies = new List<EnemyEntry>();

        int enemyCount = Mathf.Clamp(level / 10, 1, allEnemies.Count);

        for (int i = 0; i < enemyCount; i++)
        {
            EnemyEntry entry = new EnemyEntry();

            entry.enemyPrefab = allEnemies[Random.Range(0, allEnemies.Count)];

            entry.spawnChance = 100f / enemyCount;

            newConfig.enemies.Add(entry);
        }

        return newConfig;
    }
}