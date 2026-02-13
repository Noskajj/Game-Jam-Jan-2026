using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    [Header("References")]
    [SerializeField]
    private GameObject[] enemyPrefabs;
    [SerializeField]
    private GameObject player;

    private List<GameObject> spawnPoints = new List<GameObject>();

    [Header("Spawn Details")]
    [SerializeField]
    private float spawnTimer = 1f;

    

    public static event Action waveUpdated;

    #region WaveSection
    private int waveNumber = 1;
    public int WaveNumber
    {
        get => waveNumber;
    }

    private int totalWaveEnemies = 0;

    private int waveEnemiesSpawned = 0;

    private int maxEnemies = 50;

    private int enemyCount = 0;

    public void EnemyDeath()
    {
        enemyCount--;
    }
    #endregion

    private void Start()
    {
        foreach (Transform child in transform)
        {
            spawnPoints.Add(child.gameObject);
        }
    }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;

        StartCoroutine(WaveSpawning());
    }

    /// <summary>
    /// Handles the waves, including waiting between waves for all enemies to die
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaveSpawning()
    {
        while (true)
        {
            if (enemyCount > 0)
            {
                yield return new WaitForSeconds(5f);

                continue;
            }

            yield return new WaitForSeconds(2f);

            totalWaveEnemies = (int)(0.25f * Mathf.Log(waveNumber + 1) * waveNumber + 2f * waveNumber + 10f);

            waveEnemiesSpawned = 0;

            Debug.Log($"We are starting wave number {waveNumber} that should spawn {totalWaveEnemies} enemies");
            yield return StartCoroutine(EnemySpawning());

            waveNumber++;
            
        }
    }

    /// <summary>
    /// Handles spawning enemies during waves based on how many enemies in the wave
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnemySpawning()
    {
        waveUpdated?.Invoke();
        bool waveActive = true;
        while(waveActive) //For spawning enemies
        {
            yield return new WaitUntil(() => enemyCount < maxEnemies);

            //Debug.Log("Started spawn cycle");

            Vector3 selectedSpawnPos = Vector3.zero;
            bool spotFound = false;
            int attempts = 0;

            //Selects which enemy will spawn
            int selection = GetEnemySelectionIndex();

            while (!spotFound && attempts < 10) //Ensures theres no infinite loops
            {
                selectedSpawnPos = GetRandomSpawnPoint();

                Collider col = enemyPrefabs[selection].GetComponent<Collider>();

                Bounds bounds = col.bounds;

                bounds.center += selectedSpawnPos - col.transform.position;
                
                //Uses layers to determine what collisions to check for
                int spawnPointsLayer = LayerMask.NameToLayer("SpawnPoints");
                int groundLayer = LayerMask.NameToLayer("Ground");
                LayerMask ignoreMask = (1 << spawnPointsLayer) | (1 << groundLayer);

                Collider[] hits = Physics.OverlapBox(
                    bounds.center,
                    bounds.extents,
                    Quaternion.identity,
                    ~ignoreMask
                    );

                if(hits.Length == 0)
                {
                    spotFound = true;
                }

                attempts++;
            }
            
            if(spotFound)
            {
                GameObject newEnemy = Instantiate(enemyPrefabs[selection], selectedSpawnPos, enemyPrefabs[selection].transform.rotation);
                newEnemy.transform.parent = transform;

                EnemyClass enemy = newEnemy.GetComponent<EnemyClass>();
                enemy.SetPlayer(player);

                //Wave Logic 
                enemyCount++;
                waveEnemiesSpawned++;
                enemy.WaveModifiers(waveNumber);
                enemy.InitializeStun(MaskManager.Instance.mask3IsActive);
            }
            
            
        }

            yield return new WaitForSeconds(spawnTimer);

            if(waveEnemiesSpawned >= totalWaveEnemies)
                waveActive = false;
        }


   private Vector3 GetRandomSpawnPoint()
    {
        //TODO: needs to select from spawn points around the player, not just any
        Vector3 targetSpawnPoint = Vector3.zero;
        
        int i = UnityEngine.Random.Range(0, spawnPoints.Count);

        targetSpawnPoint = spawnPoints[i].transform.position;
       
        return targetSpawnPoint;
    }

    private int GetEnemySelectionIndex()
    {
        int selection = 0;

        //TODO: need to finalise spawn conditions for enemies
        //Currently after wave 5 they all spawn, after 2 ranged cultists are added
        //At the start only melee cultists

        if (waveNumber > 5)
        {
            selection = UnityEngine.Random.Range(0, enemyPrefabs.Length);
        }
        else if(waveNumber > 2)
        {
            selection = UnityEngine.Random.Range(0, 2);
        }

        //If the if statements dont successfully run, its defaulting to 0

        return selection;
    }


}
