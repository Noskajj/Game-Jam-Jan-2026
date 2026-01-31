using UnityEngine;

public class PhysicsObjects : MonoBehaviour
{
    //CONST
    const float SLEEPVEL = 0f;
    const float BUFFER = 0.02f;


    public float gravity = -20f;
    public Vector3 velocity;
    public float mass = 1f;
    public float groundCheckBuffer = 0.02f;     //Buffer for ground checks
    public LayerMask groundMask = ~0;

    public float groundFriction = 20f;
    public float sleepTime = 2f;

    //FLAGS
    public bool isGrounded;
    public bool isAsleep;


    //PREALLOCATING MEMORY
    private Rigidbody _rb;
    private Collider _col;
    private float _asleepTimer;
    float distance;

    public Vector3  Momentum => mass * velocity;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();

        _rb.useGravity = false;                 //We arnt using unity gravity

        _rb.mass = mass;
        _asleepTimer = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;

        isGrounded = Check_Grounded();

        if (isAsleep)
        {

            if (!isGrounded)
            {
                WakeUp();
            }
            return;
        }
            
            
            
            
            
            
        if (!isGrounded)
        {
            //Gravity
            velocity.y += gravity * dt;
        }
        else
        {
            velocity.y = (velocity.y < 0f) ? 0f : velocity.y;

            Friction(dt);
        }

        _rb.MovePosition(_rb.position + velocity * dt);     //S = V * detla t


        //Sleep code:

        if (isGrounded && velocity.magnitude <= 0.01f)
        {

            _asleepTimer += dt;
            if (_asleepTimer >= sleepTime)
            {
                velocity = Vector3.zero;
                Sleep();
            }


        }
        else
        {
            _asleepTimer = 0f;
        }

        
    }


    //HELPERS:__________________________
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

    public void Apply_Force(Vector3 force)
    {
        /*
         When a force is applied the rigid body wakes up
         */

        Vector3 acc = force / mass;
        velocity += acc;   //TODO: Make this a gradient

        WakeUp();
    }

    private void WakeUp()
    {
        isAsleep = false;
        _asleepTimer = 0f;
    }

    private void Sleep()
    {
        isAsleep = true;

    }

    public void Set_Velocity(Vector3 newVelocity)
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
