using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }


    [SerializeField]
    private GameObject[] enemyPrefabs;
    [SerializeField]
    private GameObject player;

    public float enemyDelay = 1f;


    #region WaveSection
    private int waveNumber = 0;

    private int totalWaveEnemies = 0;

    private int waveEnemiesSpawned = 0;

    private int maxEnemies = 50;

    private int enemyCount = 0;

    public void EnemyDeath()
    {
        enemyCount--;
    }
    #endregion


    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;

        StartCoroutine(WaveSpawning());
    }

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

    private IEnumerator EnemySpawning()
    {
        bool waveActive = true;
        while(waveActive)
        {
            yield return new WaitUntil(() => enemyCount < maxEnemies);

            //Debug.Log("Started spawn cycle");

            (int, int) coords = GetCoords();

            //Get world pos

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(coords.Item1, coords.Item2, 0f));

            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

            if(!groundPlane.Raycast(ray, out float enter))
            {
                yield return null;
                continue;
            }

            Vector3 worldPos = ray.GetPoint(enter);

            //Check navmesh
            float maxDist = 2f;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(worldPos, out hit, maxDist, NavMesh.AllAreas))
            {
                //Debug.Log("We at spawning stage");
                Vector3 spawnPos = hit.position;
                spawnPos.y = 1;
                int selection = 0;
                if(MaskManager.Instance.MasksCollected >=3)
                {
                    //Random all enemies
                    selection = Random.Range(0, enemyPrefabs.Length);
                }
                else if(MaskManager.Instance.MasksCollected >= 2)
                {
                    //Random 3 enemies
                    selection = Random.Range(0, enemyPrefabs.Length -1);
                }
                else if(MaskManager.Instance.MasksCollected >= 1)
                {
                    //Random 2 enemies
                    selection = Random.Range(0, enemyPrefabs.Length -2);
                }

                GameObject newEnemy = Instantiate(enemyPrefabs[selection], spawnPos, enemyPrefabs[selection].transform.rotation, transform);

                EnemyClass enemy = newEnemy.GetComponent<EnemyClass>();
                enemy.SetPlayer(player);

                //Wave Logic 
                enemyCount++;
                waveEnemiesSpawned++;
                enemy.WaveModifiers(waveNumber);
                enemy.InitializeStun(MaskManager.Instance.mask3IsActive);
            }

            yield return new WaitForSeconds(enemyDelay);

            if(waveEnemiesSpawned >= totalWaveEnemies)
                waveActive = false;
        }

    }

    private (int, int) GetCoords()
    {
        //Debug.Log("We getting coords");
        int side = Random.Range(0, 4);
        int x = 0;
        int y = 0;

        //Debug.Log($"{side} side");

        switch (side)
        {
            case 0:
                x = 0;
                y = Random.Range(0, Screen.height);
                break;
            case 1:
                x = Screen.width;
                y = Random.Range(0, Screen.height);
                break;
            case 2:
                y = 0;
                x = Random.Range(0, Screen.width);
                break;
            case 3:
                y = Screen.height;
                x = Random.Range(0, Screen.width);
                break;
               
        }
        

        return (x, y);
    }
}
