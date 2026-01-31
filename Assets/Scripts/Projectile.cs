using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 10f;

    [SerializeField]
    private bool specialProjectile = false;

    public int damageMult;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject,lifeTime);
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public int ProjectileHit()
    {
        int damage = PlayerStats.GunDamage;
        if(specialProjectile)
        {
            damage *= damageMult;
        }

        return damage;
    }

}
