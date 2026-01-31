using NUnit.Framework;
using UnityEngine;

public class UncScript : MonoBehaviour
{

    public GameObject projectile;

    public int curveResolution = 30;
    public float timeScale = 4f;
    public float spacing = 0.5f;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Start");
        //SpawnWave();
    }

    // Update is called once per frame
    void Update()
    {
        SpawnWave();
    }

    public void SpawnWave()
    {

        Debug.Log("Spawn trigger");
        float t = 9f;
        float radius = Mathf.Sqrt(t);

        for (int i = 0; i < curveResolution; i++)
        {
            float x = Mathf.Lerp(-radius, radius, i / (float)(curveResolution - 1));
            float y = Mathf.Sqrt(Mathf.Max(0, t - x * x));

            Vector3 localPos = new Vector3(x * spacing, 0, y * spacing);
            Vector3 worldPos = transform.TransformPoint(localPos);

            Instantiate(projectile, worldPos, Quaternion.identity);
            Debug.DrawRay(worldPos, Vector3.up, Color.red, 3f);
        }
;
    }
}
