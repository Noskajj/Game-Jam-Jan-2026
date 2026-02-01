using UnityEditor.Rendering;
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

    //RANDOM ATTACK TIMING
    public float minTimeBetween = 1f;
    public float maxTimeBetween = 4f;
    private float _atkTimer;


    //DASH NUMBERS
    public float dashForce = 100f;
    public float dashCooldown = 5f; //TODO change
    private Rigidbody _rblolt;


    //BOLT NUMBERS
    public float boltCooldown = 5f;
    public float boltLockOnWindow = 5f; //TODO change
    public float boltSpeed = 20f;
    public bool boltIsLockingOn;
    private float _boltTimer = 0f;
    public float aimAngleMod = -30f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //We arnt using unity gravity

        GetComponent<Rigidbody>().mass = bossMass;
        phys = GetComponent<PhysicsObjects>();
        _atkTimer = Random.Range(minTimeBetween, maxTimeBetween);
        _rblolt = bolt.GetComponent<Rigidbody>();
        phys.Apply_Force(new Vector3(0f, 10f, 2f));
        boltIsLockingOn = false;

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float dt = Time.fixedDeltaTime;
        if (boltIsLockingOn)
        {
            Bolt_Hold();
            Bolt_Aim();

            _boltTimer -= dt;
            if(_boltTimer <= 0f)
            {
                Bolt_Throw();
                boltIsLockingOn = false;
                _atkTimer = Random.Range(minTimeBetween, maxTimeBetween);
            }
            return;
        }
        _atkTimer -= dt;
        if (_atkTimer > 0f) return;
        {
            int attack = Random.Range(0, 2);

            if (attack == 0)
            {
                Dash_Attack();
                _atkTimer = Random.Range(minTimeBetween, maxTimeBetween);
            }
            else
            {
                Bolt_Hold();
                Bolt_Aim();
                boltIsLockingOn = true;
                _boltTimer = boltLockOnWindow;
            }

        }



        /*
         * DASH ATTACK
        timer -= Time.fixedDeltaTime;
        
        if(timer <= 0f)
        {
            Dash_Attack();
            timer = dashCooldown;
        }

        */

        /*
         * BOLT ATTACK
        timer -= Time.fixedDeltaTime;

        if (timer <= 0f)
        {
            Bolt_Hold();
            Bolt_Aim();
            if (timer < (-1f * boltLockOnWindow))
            {
                Bolt_Throw();
                timer = boltCooldown;
            }
        }
        */


    }

    void Bolt_Hold()
    {
        _rblolt.linearVelocity = Vector3.zero;
        _rblolt.MovePosition(transform.position + new Vector3 (0f, 3f, 0f));
    }

    void Bolt_Aim()
    {
        _playerDirVec = PlayerManager.Instance.transform.position - transform.position;
        Player_Unit_Vec();
        Vector3 aimVec = Quaternion.AngleAxis(aimAngleMod, Vector3.right) * _unitPlayerVec;
        bolt.Uncle_Aim(aimVec);
    }
    void Bolt_Throw()
    {
        _rblolt.linearVelocity = bolt.transform.up * boltSpeed;
    }

    void Dash_Attack()
    {
         Vector3 pointAtPlayer = PlayerManager.Instance.transform.position - transform.position;
        _playerDirVec = new Vector3(pointAtPlayer.x, 0f , pointAtPlayer.z);
        Player_Unit_Vec();

        Vector3 dashVec = _unitPlayerVec * dashForce;
        phys.Apply_Force(dashVec);

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
