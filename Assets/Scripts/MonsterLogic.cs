using UnityEngine;

public class MonsterLogic : MonoBehaviour
{
    // assign player prefeb
    public GameObject player;
    
    // monster movement
    public float monsterSpeed;
    public float stopDistance;

    //monster attacks and cooldowns
    public float meleeCooldown = 5;
    public float meleeAttacktime = 2;

    // ranged attack
    private bool isRangedAttacking;
    public float rangeAttacktime = 2;
    public float rangeAttackrange = 20;
    public float rangeCooldown = 8;
    public float rangeAttacktimer = 1;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 15;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // move towards player and stop at a set distance from the player
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (!isRangedAttacking && distance > stopDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, monsterSpeed * Time.deltaTime);
        }

        // range attack
        rangeAttacktimer -= Time.deltaTime;

        if (rangeAttacktimer <= 0)
        {
            isRangedAttacking = true;
            //Debug.Log("isRangedAttacking true");
            if (isRangedAttacking)
            {
                // range attack animation
                rangeAttacktime -= Time.deltaTime;
                //Debug.Log("rangeAttacktime reduced");
                if (rangeAttacktime <= 0)
                {
                    //spawn projectile range attack
                    GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
                    //Debug.Log("Projectile Spawned");

                    // find direction of player and rotate projectile towards player
                    Vector3 direction = (player.transform.position - firePoint.position).normalized;
                    projectile.transform.rotation = Quaternion.LookRotation(direction);

                    rangeAttacktimer = rangeCooldown;
                    rangeAttacktime = 4;
                    isRangedAttacking = false;
                }
            }

        }
        
    }
    }
