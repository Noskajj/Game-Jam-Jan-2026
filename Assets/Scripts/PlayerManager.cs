using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //Handles Attacking
    //Static shop can upgrade what you have 
    //Call of duty esqe perks around the map, max 3 equipped, set locations(maybe randomized later)

    #region Attacking

    #endregion

    #region Damage/Healing
    private Coroutine HealingStageCoroutine;

    public void HasTakenDamage()
    {
        if(HealingStageCoroutine != null) 
            StopCoroutine(HealingStageCoroutine);

        HealingStageCoroutine = StartCoroutine(DamageTaken());
    }

    private IEnumerator DamageTaken()
    {
        //Start waiting
        yield return new WaitForSeconds(PlayerStats.HealBuffer);

        //Start healing
        HealingStageCoroutine = StartCoroutine(Healing());

        yield return null;
    }

    private IEnumerator Healing()
    {
        float healBuffer = 1f / PlayerStats.HealsPerSecond;
        while (PlayerStats.Health < PlayerStats.MaxHealth)
        {
            PlayerStats.AddHealth(1);

            yield return new WaitForSeconds(healBuffer);
        }

        yield return null;
    }
    #endregion
}
