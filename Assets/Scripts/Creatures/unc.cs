using UnityEngine;

public class unc : EnemyClass
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame

    protected override void StopDist()
    {
        agent.stoppingDistance = 3f;
    }

}
