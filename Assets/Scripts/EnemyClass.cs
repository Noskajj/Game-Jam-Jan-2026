using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyClass : MonoBehaviour
{
    // assign player prefeb
    protected GameObject player;

    // monster movement
    public float monsterSpeed = 3;
    public float stopDistance = 2;

    // monster health
    public float maxHealth = 100;

    //Monster soul value
    public int soulValue = 1;

    protected bool stunned = false;

    private void Start()
    {
        MaskManager.mask3Activated += StunActivated;
        MaskManager.mask3Deactivated += StunDeactivated;
        agent = transform.GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        StopDist();
    }

    protected NavMeshAgent agent;

    protected virtual void Update()
    {
        Movement();
    }

    public void SetPlayer(GameObject playerObj)
    {
        player = playerObj;
    }

    protected virtual void Movement()
    {
        //For melee, includes stopping logic
        if (!stunned || Vector3.Distance(transform.position, player.transform.position) >= 10f)
        {
            agent.SetDestination(player.transform.position);
        }
    }

    protected virtual void StopDist()
    {
        agent.stoppingDistance = 0.5f;
    }

    private void StunActivated()
    {
        stunned = true;
    }

    private void StunDeactivated()
    {
        stunned = false;
    }

    public void InitializeStun(bool stun)
    {
        stunned = stun;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"We detecting {other.tag}");
        if (other.CompareTag("Bullet"))
        {
            // call gun damage from player stats class
            TakeDamage(other.GetComponent<Projectile>().ProjectileHit());

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
