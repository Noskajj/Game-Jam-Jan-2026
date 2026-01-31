using UnityEngine;

public abstract class EnemyClass : MonoBehaviour
{
    // assign player prefeb
    public GameObject player;

    // monster movement
    public float monsterSpeed = 3;
    public float stopDistance = 2;

    //monster attacks and cooldowns
    public float meleeCooldown = 5;
    public float meleeAttacktime = 2;

    // Melee attack
    private bool playerInMeleeRange;
    private bool isMeleeAttacking;
    public float meleeTimer;
    public int meleeDamage = 25;

    // monster health
    public float maxHealth = 100;

    //Monster soul value
    public int soulValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"We detecting {other.tag}");
        if (other.CompareTag("Bullet"))
        {
            // call gun damage from player stats class
            TakeDamage(PlayerStats.gunDamage);

            //destroys the bullet
            Destroy(other.gameObject);
        }
    }

    public bool TakeDamage(int amount)
    {
        maxHealth -= amount;
        if (maxHealth <= 0)
        {
            Death();
            return true;
        }

        return false;
    }

    public void Death()
    {
        // give player something (souls)
        PlayerStats.GainSouls(soulValue);

        //TODO: Tell spawn manager that its dead

        // destroy gameobject
        Destroy(gameObject);
        
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
                PlayerManager.Instance.HasTakenDamage(meleeDamage);
            }

            // Resets
            isMeleeAttacking = false;
            meleeTimer = meleeCooldown;
            Debug.Log("Reset melee attack");
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

    public void MeleeState()
    {
        // Don't trigger range if melee state or range is true
        if (isMeleeAttacking || playerInMeleeRange)
            return;
    }
}
