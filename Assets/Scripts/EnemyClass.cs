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

    // monster health
    public float maxHealth = 100;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            // call gun damage from player stats class
            //maxHealth -= PlayerStats.gunDamage;
        }
    }

    public void TakeDamage(int amount)
    {
        maxHealth -= amount;
        if (maxHealth <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        Debug.Log("HES FUCKIN DED");
        // gives player something (money)
        // destroy gameobject
    }
}
