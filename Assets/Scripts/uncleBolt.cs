using UnityEngine;

public class uncleBolt : MonoBehaviour
{
    public float turnSpeed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    public void Uncle_Aim(Vector3 playerUnitVect)
    {
        Debug.Log("Bolt locked on");
        Quaternion targetPlayer = Quaternion.FromToRotation(Vector3.up, playerUnitVect);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetPlayer, turnSpeed * Time.fixedDeltaTime);
    }
}
