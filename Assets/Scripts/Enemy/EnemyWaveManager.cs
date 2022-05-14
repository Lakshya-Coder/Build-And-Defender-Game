using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyWaveManager : MonoBehaviour
{
    public static EnemyWaveManager Instance { get; private set; }
    public event EventHandler OnWaveNumberChange;

    private enum State
    {
        WaitingToSpawnNextWave,
        SpawningWave
    }

    [SerializeField] private List<Transform> spawnPositionTransformList;
    [SerializeField] private Transform nextSpawnWavePositionTransform;

    private State state;
    private int waveNumber;
    private float nextWaveSpawnTimer;
    private float nextEnemySpawnTimer;
    private int remainingEnemySpawnAmount;

    private Vector3 spawnPosition;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;    
    }

    private void Start()
    {
        state = State.WaitingToSpawnNextWave;
        nextEnemySpawnTimer = 3;
        spawnPosition = spawnPositionTransformList[Random.Range(0, spawnPositionTransformList.Count)].position;
        nextSpawnWavePositionTransform.position = spawnPosition;
        SpawnWave();
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToSpawnNextWave:
                nextWaveSpawnTimer -= Time.deltaTime;
                if (nextWaveSpawnTimer <= 0f)
                    SpawnWave();

                break;
            case State.SpawningWave:
                if (remainingEnemySpawnAmount > 0)
                {
                    nextEnemySpawnTimer -= Time.deltaTime;
                    if (nextEnemySpawnTimer <= 0f)
                    {
                        nextEnemySpawnTimer = Random.Range(0f, .2f);
                        Enemy.Create(spawnPosition + UtilsClass.GetRandomDir() * Random.Range(0, 10f));
                        remainingEnemySpawnAmount--;

                        if (remainingEnemySpawnAmount <= 0)
                        {
                            state = State.WaitingToSpawnNextWave;
                            spawnPosition =
                                spawnPositionTransformList[Random.Range(0, spawnPositionTransformList.Count)].position;
                            nextSpawnWavePositionTransform.position = spawnPosition;
                            nextWaveSpawnTimer = 15f;
                        }
                    }
                }

                break;
        }
    }

    private void SpawnWave()
    {
        remainingEnemySpawnAmount = 5 + 3 * waveNumber;
        state = State.SpawningWave;
        waveNumber++;
        OnWaveNumberChange?.Invoke(this, EventArgs.Empty);
    }

    public int GetWaveNumber() => waveNumber;
    public float GetNextWaveSpawnTimer() => nextWaveSpawnTimer;

    public Vector3 GetSpawnPosition() => spawnPosition;
}