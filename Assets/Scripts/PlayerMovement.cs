using UnityEngine;
/*
    GOAL -> 

*/





[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PlayerMovement : MonoBehaviour
{
    //CONSTANT
    const float BUFFER = 0.02f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float gravity = -20f;

    public Vector3 velocity;
    
    public float mass = 1f;

    public float groundCheckBuffer = 0.02f;     //Buffer for ground checks
    public LayerMask groundMask = ~0;


    public bool is_grounded;            //Flag for if on ground


    //PREALLOCATING MEMORY
    private Rigidbody _rb;
    private Collider _col;
    float distance;

    public Vector3  Momentum => mass * velocity;




     
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();

        _rb.useGravity = false;                 //We arnt using unity gravity

        _rb.mass = mass;

        SetVelocity(new Vector3(0, 5, 0));

    }

    // Update is called once per frame
    void Update()
    {
        float _dt = Time.deltaTime;

        is_grounded = CheckGrounded();

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


         _rb.MovePosition(_rb.position + velocity * _dt);   // S = V*t


    }



    //HELPERS:__________________________

    public void SetVelocity(Vector3 newVelocity)
    {
        /*
        Allows us to update velocity 
        */

        velocity = newVelocity;
    } 
    
    private bool CheckGrounded()
    {
        
        Bounds b = _col.bounds;

        Vector3 origin = new Vector3(b.center.x, b.min.y + BUFFER, b.center.z);


        return Physics.Raycast(
            origin,
            Vector3.down,
            (BUFFER + groundCheckBuffer),
            groundMask,
            QueryTriggerInteraction.Ignore
        );
    }
}
