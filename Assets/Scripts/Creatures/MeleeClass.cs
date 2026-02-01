using UnityEngine;

public class MeleeClass : EnemyClass
{
    //monster attacks and cooldowns
    public float meleeCooldown = 5;
    public float meleeAttacktime = 2;

    // Melee attack
    private bool playerInMeleeRange;
    private bool isMeleeAttacking;
    public float meleeTimer;
    public int meleeDamage = 25;

    public void MeleeCheck()
    {
        if (!playerInMeleeRange)
            return;

        if (!isMeleeAttacking)
        {
            isMeleeAttacking = true;
            //Play enemyMelee sound
            AudioManager.instance.PlayOneShot(FMODEvents.instance.monsterMelee, this.transform.position);
            //Debug.Log("Melee attacking");
            meleeTimer = meleeAttacktime;
        }

        meleeTimer -= Time.deltaTime;

        if (meleeTimer <= 0)
        {
            if (playerInMeleeRange)
            {
                // Player take damage
                //Debug.Log("Player took damage");
                //Play playerHurt sound
                AudioManager.instance.PlayOneShot(FMODEvents.instance.playerHurt, this.transform.position);
                PlayerManager.Instance.HasTakenDamage(meleeDamage);
            }

            // Resets
            isMeleeAttacking = false;
            meleeTimer = meleeCooldown;
            //Debug.Log("Reset melee attack");
        }
    }
    public void PlayerEnteredMeleeRange()
    {
        playerInMeleeRange = true;
        //Debug.Log("Player in melee range");
    }

    public void PlayerExitMeleeRange()
    {
        playerInMeleeRange = false;
        //Debug.Log("Player exit melee range");
        // Cancel melee
        isMeleeAttacking = false;
    }

    public void MeleeState()
    {
        // Don't trigger range if melee state or range is true
        if (isMeleeAttacking || playerInMeleeRange)
            return;
    }
}
