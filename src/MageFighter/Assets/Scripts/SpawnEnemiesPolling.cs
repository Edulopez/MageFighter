using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnEnemiesPolling : ObjectPooling
{

    private int _lastSpawnPosition = -1;
    public List<GameObject> spawnPositions;

    public int SpawnInterval = 3;
    private float _lastSPawnTime = 0;
    private bool _isSpawning = false;
    
    GameObject GetSpawnPosition()
    {
        if (spawnPositions == null || spawnPositions.Count == 0)
            return null;
        int position = Random.Range(0, spawnPositions.Count);
        var spawn = spawnPositions[position];
        Debug.Log("Spawing on " + position);

        return spawn;
    }

    IEnumerator SpawnEnemy(int index, float seconds)
    {
        var spawn = GetSpawnPosition();

        if (spawn == null)
            yield break;

        var enemy = GetPooledObject();
        if (enemy == null)
            yield break;
        yield return new WaitForSeconds(seconds);
        Instantiate(enemy, spawn.transform.position, spawn.transform.rotation);
    
        _isSpawning = false;
    }

    void Update()
    {

        if (!_isSpawning)
        {
            _isSpawning = true;
            StartCoroutine(SpawnEnemy(Random.Range(0, spawnPositions.Count), SpawnInterval));
        }
    }
}
