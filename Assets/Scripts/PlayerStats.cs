using System;
using UnityEngine;

public static class PlayerStats
{
    #region Delegates
    public static event Action MaxHealthChanged, healthUpdated, soulsUpdated, ammoUpdated;
    #endregion

    #region Variables
    private static int speed = 5;
    public static int Speed 
    {
        //speed + or * upgrades
        get => speed;
    }

    private static int maxHealth = 75;
    public static int MaxHealth
    {
        get => maxHealth;
        set
        {
            maxHealth = value;
            MaxHealthChanged?.Invoke();
        }
    }

    private static int health = 75;
    public static int Health
    { 
        get => health;
    }

    private static int souls = 0;
    public static int Souls
    {
        get => souls;
    }

    private static int ammo = 6;
    public static int Ammo
    {
        get => ammo;
    }

    private static int maxStamina = 100;
    public static int MaxStamina
    {
        get => maxStamina;
    }

    private static int currentStamina = 100;
    public static int CurrentStamina
    {
        get => currentStamina;
    }

    public static float HealBuffer = 2f;

    public static float HealsPerSecond = 10f;

    public static float StamBuffer = 1f;
    public static float StamRegenPerSecond = 10f;

    private static int meleeDamage = 25;
    public static int meleeBonus = 0;
    public static int MeleeDamage
    {
        get => meleeDamage + meleeBonus;
    }
    private static int gunDamage = 25;
    public static int gunBonus = 0;
    public static int GunDamage
    {
        get => gunDamage + gunBonus;
    }

    private static int maxGunAmmo = 6;
    public static int MaxGunAmmo
    {
        get => maxGunAmmo;
    }

    private static int currentAmmo = 6;
    public static int CurrentAmmo
    {
        get => currentAmmo;
    }

    public static float ReloadSpeed = 5f;

    public static int TotalAmmo = 30;

    public static float meleeBuffer = 0.5f;
    public static float gunBuffer = 0.5f;
    #endregion

    #region Functions

    public static void AddHealth(int healthToAdd)
    {
        if(health >= maxHealth)
            return;

        health = Math.Min(maxHealth, health + healthToAdd);
        healthUpdated?.Invoke();
    }

    /// <summary>
    /// Removes health from the player
    /// </summary>
    /// <param name="healthToRemove"></param>
    /// <returns>True = dead, False = alive</returns>
    public static bool RemoveHealth(int healthToRemove)
    {
        if (health <= 0)
            return true;

        health = Math.Max(0, health - healthToRemove);
        healthUpdated?.Invoke();

        if (health <= 0)
            return true;
       

        return false;
    }

    public static bool CanPurchase(int purchaseCost)
    {
        if(Souls >= purchaseCost)
            return true;
        else
            return false;
    }

    public static void SpendSouls(int purchaseCost)
    {
        souls = Math.Max(0, souls - purchaseCost);
        soulsUpdated?.Invoke();
    }

    public static void GainSouls(int gainAmount)
    {
        souls += gainAmount;
        soulsUpdated?.Invoke();
    }

    public static void GainAmmo(int gainAmount)
    {
        ammo += gainAmount;
        ammoUpdated?.Invoke();
    }

    public static void AmmoUsed()
    {
        ammo--;
        ammoUpdated?.Invoke();
    }

    public static bool HasEnoughStamina(int amount)
    {
        if(currentStamina >= amount)
        {
            return true;
        }
        else
            return false;
    }

    public static void UseStamina(int amount)
    {
        currentStamina -= amount;
    }

    public static void GainStamina(int amount)
    {
        currentStamina += amount;
    }

    public static void ShootGun()
    {
        currentAmmo--;
    }

    public static void ReloadGun()
    {
        //Need to do 
        if (TotalAmmo <= 0 || currentAmmo >= 6)
            return;

        TotalAmmo--;
        currentAmmo++;
    }

    #endregion
}
