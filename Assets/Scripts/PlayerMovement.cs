using UnityEngine;
/*
    GOAL -> 

*/

//CONSTANT
const float BUFFER = 0.02f;



[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header ("Gravity")]
    public float gravity = -20f;

    [Header ("Velocity")]
    public Vector3  velocity;

    [Head ("Ground Check Buffer")]
    float groundCheckBuffer = 0.02f;     //Buffer for ground checks

    public bool is_grounded;            //Flag for if on ground


    //PREALLOCATING MEMORY
    private RigidBody _rb;
    private Collider _col;
    private float _dt;
    float distance;

    public Vector3  Momentum => mass * velocity;




     
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();

        _rb.useGravity = false;                 //We arnt using unity gravity

        _rb.mass = mass;

    }

    // Update is called once per frame
    void Update()
    {
        is_grounded = _CheckGrounded();

        //Gravity 
        if (!is_grounded)
        { 
            velocity.y += gravity * _dt;         //V = u + a*t
        }
        else
        {
            //TODO: See if this is needed
            //Ensures velocity doesnt build up when on ground
            if (velocity.y < 0f) velocity.y = 0f;
        }


         _rb.MovePosition(_rb.position + velocity * _dt);


    }



    //HELPERS:__________________________

    public void SetVelocity(Vector3 newVelocity)
    {
        /*
        Allows us to update velocity 
        */

        velocity = newVelocity;
    } 

    private bool _CheckGrounded()
    {
        
        Bounds b = _col.bounds;

        Vector3 origin = new Vector3(b.center.x, b.min.y + BUFFER, b.center.z);

        return Physics.Raycast(origin, Vector3.down ,groundCheckBuffer, 0, QueryTriggerInteraction.Ignore);

    }
}
