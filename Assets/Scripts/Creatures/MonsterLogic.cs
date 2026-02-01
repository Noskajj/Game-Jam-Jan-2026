using System.Xml.Serialization;
using UnityEngine;

public class MonsterLogic : MeleeClass
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
 
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        
            RangeCheck();
            MeleeCheck();
        
    }

    private void ShootProjectile()
    {
        //Play knifeThrow sound
        AudioManager.instance.PlayOneShot(FMODEvents.instance.knifeThrow, this.transform.position);
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
        MeleeState();

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
                //Debug.Log("Projectile Spawned");
                rangeAttacktimer = rangeCooldown;
                isRangedAttacking = false;
            }
        }
    }

    
}
