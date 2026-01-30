using UnityEngine;
/*
    GOAL -> 

*/



[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header ("Gravity")]
    public float gravity = -20f;

    [Header ("Velocity")]
    public Vel velocity;

    public bool is_grounded;            //Flag for if on ground

     
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
