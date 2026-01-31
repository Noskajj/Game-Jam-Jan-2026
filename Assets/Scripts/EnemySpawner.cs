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

            //Get world pos

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(coords.Item1, coords.Item2, 0f));

            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

            if(!groundPlane.Raycast(ray, out float enter))
            {

            }

            Vector3 worldPos = ray.GetPoint(enter);

            //Check navmesh
            float maxDist = 2f;
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

        Debug.Log($"{side} side");

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
