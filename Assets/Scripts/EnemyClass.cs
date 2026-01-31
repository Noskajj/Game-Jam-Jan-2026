using UnityEngine;

public abstract class EnemyClass : MonoBehaviour
{
    // assign player prefeb
    public GameObject player;

    // monster movement
    public float monsterSpeed = 3;
    public float stopDistance = 2;

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
        EnemySpawner.Instance.EnemyDeath();

        // destroy gameobject
        Destroy(gameObject);
        
    }

    public void WaveModifiers(int wave)
    {
        maxHealth = 15f * Mathf.Log(wave + 1) + 75;
        monsterSpeed *= 0.12f * Mathf.Log(wave + 1) + 1;

    }

}
