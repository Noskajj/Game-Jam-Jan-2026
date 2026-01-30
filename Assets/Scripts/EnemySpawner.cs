using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject player;

    public float enemyDelay = 1f;

    private Coroutine spawnEnemyCoroutine;


    private void Awake()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        bool currentWave = true;
        while(currentWave)
        {
            Debug.Log("Started spawn cycle");

            (int, int) coords = GetCoords();

            //Get screen edges
            Vector3 screenPos = new Vector3(coords.Item1, coords.Item2, Camera.main.transform.position.y);

            //Convert to world pos
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
                
            worldPos.y = 0;

            //Check navmesh
            float maxDist = 0.5f;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(worldPos, out hit, maxDist, NavMesh.AllAreas))
            {
                Debug.Log("We at spawning stage");
                Vector3 spawnPos = hit.position;
                spawnPos.y = 1;
                GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity, transform);
                newEnemy.GetComponent<EnemyClass>().player = player;
            }

            yield return new WaitForSeconds(enemyDelay);
        }

    }

    private (int, int) GetCoords()
    {
        Debug.Log("We getting coords");
        int side = Random.Range(0, 4);
        int x = 0;
        int y = 0;

        switch (side)
        {
            case 0:
                y = Random.Range(0, Screen.height);
                break;
            case 1:
                x = Screen.width;
                y = Random.Range(0, Screen.height);
                break;
            case 2:
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
