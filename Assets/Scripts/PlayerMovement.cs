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
    
    public float playerAccel = 30f;

    public float jumpVel = 8f;
    
    public float mass = 1f;

    public float groundCheckBuffer = 0.02f;     //Buffer for ground checks
    public LayerMask groundMask = ~3;           //1111111111111111100
    private int groundContacts = 0;


    public bool isGrounded;            //Flag for if on ground

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

        Set_Velocity(new Vector3(0, 5, 0));

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        

        float _dt = Time.fixedDeltaTime;
        isGrounded = groundContacts > 0;
        
        Player_Move(_dt);



        //Gravity 
        if (!isGrounded)
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


         _rb.linearVelocity = velocity;   // S = V*t


    }



    //HELPERS:__________________________

    private void Player_Move(float _dt)
    {
        //Apply acceleration in a direction
        // V_{t+1} = V_t + clamp((U * MAX_SPEED), -r*delta_t, r*delta_t)

        Vector2 keyInput = moveAction.ReadValue<Vector2>();

        //Gives the current velocity vector in 3D space
        Vector3 currVel = new Vector3(velocity.x, 0f, velocity.z);

        if (keyInput.sqrMagnitude > 0f)
        {
            //Gives a velocity vector in 3D space
            Vector3 targVel = new Vector3(keyInput.x, 0f, keyInput.y) * moveSpeed;

            currVel = Vector3.MoveTowards(currVel, targVel, playerAccel * _dt);
        }

        velocity.x = currVel.x;
        velocity.z = currVel.z;



    }


    private float Clamp(float x, float a, float b)
    {
        /*
            Clamps the value of x between a given range of a-b.
            Used for physics calculations
        */

        if (x < a)
        {
            return a;
        }
        else if (a <= x && x <= b)
        {
            return x;
        }
        else
        {
            return b;
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

    public void Set_Velocity(Vector3 newVelocity)
    {
        /*
        Allows us to update velocity 
        */

        velocity = newVelocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Collisions call back. 

        //Bitwise operations to check if correct layer. Very efficent
        if (((1 << collision.gameObject.layer) & groundMask) != 0)
        {
            groundContacts++;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //Bitwise operations to check if correct layer. Very efficent
        if (((1 << collision.gameObject.layer) & groundMask) != 0)
        {
            groundContacts--;
        }
    }
}

