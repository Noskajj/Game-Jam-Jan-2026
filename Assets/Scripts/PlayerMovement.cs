using UnityEngine;
using UnityEngine.InputSystem;
/*
    GOAL -> 

*/





[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PlayerMovement : MonoBehaviour
{
    //CONSTANT
    const float BUFFER = 0.02f;

    [SerializeField]
    private InputActionAsset inputActions;
    private InputAction moveAction;
    private InputAction jumpAction;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float gravity = -20f;

    public Vector3 velocity;

    public float moveSpeed = 4f;

    public float jumpVel = 8f;
    
    public float mass = 1f;

    public float groundCheckBuffer = 0.02f;     //Buffer for ground checks
    public LayerMask groundMask = ~0;


    public bool is_grounded;            //Flag for if on ground

    //TODO: Make surface share this instead of hard coding
    public float groundFriction = 20f;



    //PREALLOCATING MEMORY
    private Rigidbody _rb;
    private Collider _col;
    float distance;

    public Vector3  Momentum => mass * velocity;




     
    void Start()
    {
        moveAction = inputActions.FindAction("Player/Move");
        jumpAction = inputActions.FindAction("Player/Jump");
        
        jumpAction.performed += Player_Jump;


        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();

        _rb.useGravity = false;                 //We arnt using unity gravity

        _rb.mass = mass;

        SetVelocity(new Vector3(0, 5, 0));

    }

    // Update is called once per frame
    void Update()
    {
        
        Player_Move();


        float _dt = Time.deltaTime;

        is_grounded = Check_Grounded();

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
            Friction(_dt);
        }


         _rb.MovePosition(_rb.position + velocity * _dt);   // S = V*t


    }



    //HELPERS:__________________________

    private void Player_Move()
    {
        Vector2 movDirection = moveAction.ReadValue<Vector2>();;
        if(movDirection.x != 0)
        {
            velocity.x = movDirection.x * moveSpeed;
        }
        if(movDirection.y != 0)
        {
            velocity.z = movDirection.y * moveSpeed;
        }

    }

    private void Player_Jump(InputAction.CallbackContext context)
    {
        float jump = context.ReadValue<float>();
        if(jump != 0){
            velocity.y = jumpVel;
        }
    }


    private void Friction(float _dt)
    {
        Vector3 horzVel = new Vector3(velocity.x, 0f, velocity.z);
        //This takes the current Velocity vector, and changes the values to approach 0,0,0, 
        //at the increment of the time passed
        horzVel = Vector3.MoveTowards(
            horzVel,
            Vector3.zero,
            groundFriction * _dt
        );

        velocity.x = horzVel.x;
        velocity.z = horzVel.z;

    }

    public void SetVelocity(Vector3 newVelocity)
    {
        /*
        Allows us to update velocity 
        */

        velocity = newVelocity;
    } 
    
    private bool Check_Grounded()
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
