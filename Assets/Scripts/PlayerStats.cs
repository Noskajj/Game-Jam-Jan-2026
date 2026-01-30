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

    private static int maxHealth = 100;
    public static int MaxHealth
    {
        get => maxHealth;
        set
        {
            maxHealth = value;
            MaxHealthChanged?.Invoke();
        }
    }

    private static int health = 100;
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
    #endregion

    #region Functions

    public static void AddHealth(int healthToAdd)
    {
        if(health >= maxHealth)
            return;

        health = Math.Min(maxHealth, health + healthToAdd);
        healthUpdated?.Invoke();
    }

    public static void RemoveHealth(int healthToRemove)
    {
        if (health <= 0)
            return;

        health = Math.Max(0, health - healthToRemove);
        healthUpdated?.Invoke();
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

    #endregion
}
