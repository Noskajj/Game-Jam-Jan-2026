using System.Xml.Serialization;
using UnityEngine;

public class MonsterLogic : EnemyClass
{

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
        RangeCheck();
    }

    private void ShootProjectile()
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

    private void RangeCheck()
    {
        // move towards player and stop at a set distance from the player
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (!isRangedAttacking && distance > stopDistance)
        {
            Vector3 playerPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, playerPos, monsterSpeed * Time.deltaTime);
        }

        // range attack
        rangeAttacktimer -= Time.deltaTime;
        float distanceFromplayer = Vector3.Distance(transform.position, player.transform.position);
        if (rangeAttacktimer <= 0 && distanceFromplayer >= 10)
        {
            isRangedAttacking = true;
            Debug.Log("isRangedAttacking true");
            if (isRangedAttacking)
            {
                // range attack animation
                rangeAttacktime -= Time.deltaTime;
                Debug.Log("rangeAttacktime reduced");
                if (rangeAttacktime <= 0)
                {
                    ShootProjectile();
                    Debug.Log("Projectile Spawned");
                }
            }
            else
            {
                rangeAttacktimer = rangeCooldown;
                rangeAttacktime = 4;
                isRangedAttacking = false;
                Debug.Log("rangeAttack Reset");
            }
        }
    }
}
