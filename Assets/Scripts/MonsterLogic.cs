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

    // Melee attack
    private bool isMeleeAttacking;
    private bool playerInMeleeRange;
    public float meleeTimer;
    public int meleeDamage = 10;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    
    // Update is called once per frame
    void Update()
    {
        RangeCheck();
        MeleeCheck();
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
        // Don't trigger range if melee state or range is true
        if (isMeleeAttacking || playerInMeleeRange)
            return;
        
        // move towards player and stop at a set distance from the player
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (!isRangedAttacking && distance > stopDistance)
        {
            // find player position
            Vector3 playerPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            // move towards player position
            transform.position = Vector3.MoveTowards(transform.position, playerPos, monsterSpeed * Time.deltaTime);
        }

        // range attack
        if (!isRangedAttacking)
        {
            rangeAttacktimer -= Time.deltaTime;
            if (rangeAttacktimer <= 0 && distance >= 10)
            {
                isRangedAttacking = true;
                rangeAttacktime = 4;
            }
        }
        else
        {
            rangeAttacktime -= Time.deltaTime;
            if (rangeAttacktime <= 0)
            {
                ShootProjectile();
                Debug.Log("Projectile Spawned");
                rangeAttacktimer = rangeCooldown;
                isRangedAttacking = false;
            }
        }
    }

    public void PlayerEnteredMeleeRange()
    {
        playerInMeleeRange = true;
        Debug.Log("Player in melee range");  
    }

    public void PlayerExitMeleeRange()
    {
        playerInMeleeRange = false;
        Debug.Log("Player exit melee range");
        // Cancel melee
        isMeleeAttacking = false;
    }

    public void MeleeCheck()
    {
        if (!playerInMeleeRange)
            return;

        if (!isMeleeAttacking)
        {
            isMeleeAttacking = true;
            Debug.Log("Melee attacking");
            meleeTimer = meleeAttacktime;
        }

        meleeTimer -= Time.deltaTime;

        if (meleeTimer <= 0)
        {
            if (playerInMeleeRange)
            {
                // Player take damage
                Debug.Log("Player took damage");
            }

            // Resets
            isMeleeAttacking = false;
            meleeTimer = meleeCooldown;
            Debug.Log("Reset melee attack");
        }
    }
}
