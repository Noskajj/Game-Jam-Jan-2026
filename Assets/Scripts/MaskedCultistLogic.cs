using System.Xml.Serialization;
using UnityEngine;

public class MaskedCultistLogic : MeleeClass
{
    private float distance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        if (!stunned || Vector3.Distance(transform.position, player.transform.position) >= 10f)
        {
            DistanceCheck();
            MeleeCheck();
        }
    }

    private void DistanceCheck()
    {
        MeleeState();

        
        // move towards player and stop at a set distance from the player
        distance = Vector3.Distance(transform.position, player.transform.position);
       

        if (distance > stopDistance)
        {
            // find player position
            Vector3 playerPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            // move towards player position
            transform.position = Vector3.MoveTowards(transform.position, playerPos, monsterSpeed * Time.deltaTime);
        }
    }


}
