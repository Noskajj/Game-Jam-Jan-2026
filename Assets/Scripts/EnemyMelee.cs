using UnityEngine;

public class EnemyMelee : MonoBehaviour
{

    private EnemyClass enemyClass;
    private void Awake()
    {
        enemyClass = GetComponentInParent<EnemyClass>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyClass.PlayerEnteredMeleeRange();
            Debug.Log("Melee hitbox colliding with player");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyClass.PlayerExitMeleeRange();
        }
    }
}
