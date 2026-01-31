using UnityEngine;
using UnityEngine.Video;

public class uncleAttacks : MonoBehaviour
{

    private PhysicsObjects phys;
    //private uncleBolt bolt;


    private Vector3 _playerDirVec = Vector3.zero;
    private Vector3 _unitPlayerVec = Vector3.zero;

    [SerializeField] public uncleBolt bolt;

    public float bossMass = 20f;

    public float timer = 0f; //TODO get rid of


    //DASH NUMBERS
    public float dashForce = 100f;
    public float dashCooldown = 3f; //TODO change



    //BOLT NUMBERS
    public float boltCooldown = 0.1f;
    public float boltLockOnWindow = 5f; //TODO change


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //We arnt using unity gravity

        GetComponent<Rigidbody>().mass = bossMass;
        phys = GetComponent<PhysicsObjects>();
        timer = dashCooldown;



    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        timer -= Time.fixedDeltaTime;
        
        if(timer <= 0f)
        {
            Dash_Attack();
            timer = dashCooldown;
        }

        

        /*
        timer -= Time.fixedDeltaTime;

        if (timer <= 0f)
        {
            Bolt_Attack();
            if (timer < (-1f * boltLockOnWindow))
            {
                timer = boltCooldown;
            }
        }


        */
    }


    void Bolt_Attack()
    {
        _playerDirVec = PlayerManager.Instance.transform.position - transform.position;
        Player_Unit_Vec();
        bolt.Uncle_Aim(_unitPlayerVec);
    }


    void Dash_Attack()
    {
         Vector3 pointAtPlayer = PlayerManager.Instance.transform.position - transform.position;
        _playerDirVec = new Vector3(pointAtPlayer.x, 0f, pointAtPlayer.z);
        Player_Unit_Vec();


        phys.Apply_Force(_unitPlayerVec * dashForce);

    }


    void Player_Unit_Vec()
    {
        float sqrDist = _playerDirVec.sqrMagnitude;

        if (sqrDist > 0.0001f)
        {
            _unitPlayerVec = _playerDirVec / Mathf.Sqrt(sqrDist);
        }
        else
        {
            _unitPlayerVec = Vector3.zero;
        }

    }


}
