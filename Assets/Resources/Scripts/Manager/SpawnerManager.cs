using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : Singleton<SpawnerManager>
{
    [SerializeField] private Transform[] spawnPoints;

    private void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
    }

    public Transform GetSpawnPoint()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        
        return spawnPoints[spawnIndex];
    }
}
