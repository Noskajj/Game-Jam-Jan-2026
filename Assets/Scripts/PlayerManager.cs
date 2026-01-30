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
    
    private InputAction equipGunAction, attackAction;
    private void Start()
    {
        equipGunAction = InputSystem.actions.FindAction("EquipGun");
        attackAction = InputSystem.actions.FindAction("Attack");
        equipGunAction.started += EquipGun;
        equipGunAction.canceled += UnequipGun;
        attackAction.performed += Attack;
       
    }

    #region ProjectileDetection
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Projectile"))
        {
            HasTakenDamage(10);
            Destroy(other.gameObject);
        }
    }
    #endregion

    #region Attacking
    private bool gunEquiped = false;

    private void EquipGun(InputAction.CallbackContext context)
    {
        gunEquiped = true;
    }

    private void UnequipGun(InputAction.CallbackContext context)
    {
        gunEquiped = false;
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if(!gunEquiped)
        {

        }
        else
        {

        }
    }
    #endregion

    #region Damage/Healing
    private Coroutine HealingStageCoroutine;


    public void HasTakenDamage(int damage)
    {
        if(PlayerStats.RemoveHealth(damage))
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

    #region Stamina
    private Coroutine StaminaRegenCoroutine;


    public void HasUsedStamina(int amount)
    {
        if (StaminaRegenCoroutine != null)
            StopCoroutine(StaminaRegenCoroutine);

        StaminaRegenCoroutine = StartCoroutine(StaminaUsed());
    }

    private IEnumerator StaminaUsed()
    {
        //Start waiting
        Debug.Log("We waiting for Stam cooldown");
        yield return new WaitForSeconds(PlayerStats.StamBuffer);

        //Start healing
        StaminaRegenCoroutine = StartCoroutine(StaminaRegen());

        yield return null;
    }

    private IEnumerator StaminaRegen()
    {
        float currentStamTimer = 0;
        float stamBuffer = 1f / PlayerStats.StamRegenPerSecond;
        while (PlayerStats.CurrentStamina < PlayerStats.MaxStamina)
        {
            Debug.Log($"We are at {PlayerStats.CurrentStamina} stam out of {PlayerStats.MaxStamina}. With {currentStamTimer} seconds elapsed");
            PlayerStats.GainStamina(1);


            currentStamTimer += stamBuffer;
            yield return new WaitForSeconds(stamBuffer);
        }

        yield return null;
    }
    #endregion
}
