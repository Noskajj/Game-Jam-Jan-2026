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
    protected override void Update()
    {
        base.Update();
        RangeCheck();
    }

    private void ShootProjectile()
    {
        //Play enemyMagicAttack sound
        AudioManager.instance.PlayOneShot(FMODEvents.instance.enemyMagicAttack, this.transform.position);
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

    protected override void StopDist()
    {
        agent.stoppingDistance = stopDistance;
    }

    private void RangeCheck()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
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
}
