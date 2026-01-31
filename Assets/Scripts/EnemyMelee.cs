using UnityEngine;

public class EnemyMelee : MonoBehaviour
{

    private MonsterLogic monsterLogic;
    private void Awake()
    {
        monsterLogic = GetComponentInParent<MonsterLogic>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            monsterLogic.PlayerEnteredMeleeRange();
            Debug.Log("Melee hitbox colliding with player");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            monsterLogic.PlayerExitMeleeRange();
        }
    }
}
