using UnityEngine;

public class RangedCultistLogic : EnemyClass
{
    // ranged attack
    private bool isRangedAttacking;
    public float rangeAttacktime = 2;
    public float rangeCooldown = 3;
    public float rangeAttacktimer = 3;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 25;

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
            // find player position
            Vector3 playerPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            // move towards player position
            transform.position = Vector3.MoveTowards(transform.position, playerPos, monsterSpeed * Time.deltaTime);
        }

        // range attack
        if (!isRangedAttacking)
        {
            rangeAttacktimer -= Time.deltaTime;
            if (rangeAttacktimer <= 0)
            {
                isRangedAttacking = true;
                rangeAttacktime = 2;
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
