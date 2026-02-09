using System.Collections;
using System.Threading;
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

    [SerializeField]
    private SpriteRenderer enemyDamageOverlay;
    private float enemyDamageFadeTime = 0.5f;
    private Coroutine damageVisualisationCoroutine;

    private void Start()
    {
        MaskManager.mask3Activated += StunActivated;
        MaskManager.mask3Deactivated += StunDeactivated;
        agent = transform.GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        StopDist();
    }

    private void OnDisable()
    {
        MaskManager.mask3Activated -= StunActivated;
        MaskManager.mask3Deactivated -= StunDeactivated;
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
            // Play enemyHurt sound
            AudioManager.instance.PlayOneShot(FMODEvents.instance.enemyHurt, this.transform.position);
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

        if(damageVisualisationCoroutine != null) 
            StopCoroutine(damageVisualisationCoroutine);

        Debug.Log("Enemy should be taking damage");
        damageVisualisationCoroutine = StartCoroutine(DamageVisualisation());

        return false;
    }

    public void Death()
    {
        // give player something (souls)
        PlayerStats.GainSouls(soulValue);

        //Tell spawn manager that its dead
        EnemySpawner.Instance.EnemyDeath();

        // destroy gameobject
        Destroy(gameObject);
        
    }

    public void WaveModifiers(int wave)
    {
        maxHealth *= Mathf.Atan(wave * 0.02f) + 1f;
        monsterSpeed *= 0.12f * Mathf.Log(wave) + 1;

    }

    private IEnumerator DamageVisualisation()
    {
        //fade in
        
        float timer = 0;
        Debug.Log($"Enemy: We starting the visu enumer timer:{timer} fade time:{enemyDamageFadeTime}");
        while (timer < enemyDamageFadeTime)
        {
            timer += Time.deltaTime;
            float t = timer / enemyDamageFadeTime;

            var col = enemyDamageOverlay.color;
            col.a = Mathf.Lerp(0, 1, t);
            enemyDamageOverlay.color = col;
            yield return null;
        }

        //leave maxxed for a second
        yield return new WaitForSeconds(1f);

        //End effect
        timer = 0;

        


        while (timer < enemyDamageFadeTime)
        {
            timer += Time.deltaTime;
            float t = timer / enemyDamageFadeTime;

            var col = enemyDamageOverlay.color;
            col.a = Mathf.Lerp(1, 0, t);
            enemyDamageOverlay.color = col;

            yield return null;
        }

        var c = enemyDamageOverlay.color;
        c.a = 0;
        enemyDamageOverlay.color = c;
    }

}
