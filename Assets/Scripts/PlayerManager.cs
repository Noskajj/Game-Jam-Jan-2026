using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    //Handles Attacking
    //Static shop can upgrade what you have 
    //Call of duty esqe perks around the map, max 3 equipped, set locations(maybe randomized later)

    //TODO: Stamina bar thats the same as health system
    [SerializeField]
    private InputActionAsset inputActions;
    private InputAction action;
    private void Start()
    {
        action = inputActions.FindAction("Player/Attack");
        action.performed += TestDamage;
    }

    private void TestDamage(InputAction.CallbackContext context)
    {
        HasTakenDamage();
    }

    #region Attacking

    #endregion

    #region Damage/Healing
    private Coroutine HealingStageCoroutine;


    public void HasTakenDamage()
    {
        //TODO: Range does less damage than melee
        if(PlayerStats.RemoveHealth(25))
        {
            //TODO: Player is dead, run dead code
            return;
        }

        if(HealingStageCoroutine != null) 
            StopCoroutine(HealingStageCoroutine);

        HealingStageCoroutine = StartCoroutine(DamageTaken());
    }

    private IEnumerator DamageTaken()
    {
        //Start waiting
        Debug.Log("We waiting for healing cooldown");
        yield return new WaitForSeconds(PlayerStats.HealBuffer);

        //Start healing
        HealingStageCoroutine = StartCoroutine(Healing());

        yield return null;
    }

    private IEnumerator Healing()
    {
        float currentHealTimer = 0;
        float healBuffer = 1f / PlayerStats.HealsPerSecond;
        while (PlayerStats.Health < PlayerStats.MaxHealth)
        {
            Debug.Log($"We are at {PlayerStats.Health} health out of {PlayerStats.MaxHealth}. With {currentHealTimer} seconds elapsed");
            PlayerStats.AddHealth(1);


            currentHealTimer += healBuffer;
            yield return new WaitForSeconds(healBuffer);
        }

        yield return null;
    }
    #endregion
}
