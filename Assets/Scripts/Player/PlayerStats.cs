using System;
using UnityEngine;

public static class PlayerStats
{
    #region Delegates
    public static event Action MaxHealthChanged, healthUpdated, soulsUpdated, ammoUpdated;
    #endregion

    #region Variables
    private static int speed = 5;
    public static float speedBonus = 0;
    public static float Speed 
    {
        //speed + or * upgrades
        get => speed + speedBonus;
    }

    private static int maxHealth = 75;
    private static int maxHealthBonus = 0;
    public static int MaxHealth
    {
        get => maxHealth + maxHealthBonus;
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
    private static int bonusMaxStamina = 0;
    public static int MaxStamina
    {
        get => maxStamina + bonusMaxStamina;
    }

    private static int currentStamina = 100;
    public static int CurrentStamina
    {
        get => currentStamina;
    }

    private static int swordStaminaCost = 25;
    public static int SwordStaminaCost
    {
        get => Math.Max(5, swordStaminaCost - UpgradeManager.Instance.EfficientSwing);
    }
    public static int dashStaminaCost = 25;
    private static float healBuffer = 2f;
    private static float bonusHealBuffer = 0f;
    public static float HealBuffer
    {
        get => healBuffer - bonusHealBuffer;
    }

    public static float HealsPerSecond = 10f;

    private static float stamBuffer = 1f;
    private static float bonusStamBuffer = 0f;
    public static float StamBuffer
    {
        get => stamBuffer - bonusStamBuffer;
    }

    public static float StamRegenPerSecond = 10f;

    private static int meleeDamage = 25;
    public static int meleeBonus = 0;
    public static int MeleeDamage
    {
        get => meleeDamage + meleeBonus + UpgradeManager.Instance.HarderSwings;
    }
    private static int gunDamage = 25;
    public static int gunBonus = 0;
    public static int GunDamage
    {
        get => gunDamage + gunBonus + UpgradeManager.Instance.HarderBullet;
    }

    private static int maxGunAmmo = 6;
    public static int MaxGunAmmo
    {
        get => maxGunAmmo + UpgradeManager.Instance.LargerChamber;
    }

    private static int currentAmmo = 6;
    public static int CurrentAmmo
    {
        get => currentAmmo;
    }

    private static float reloadSpeed = 5f;
    public static float ReloadSpeed
    {
        get => Math.Max(1f, reloadSpeed - UpgradeManager.Instance.NimbleFingers);
    }

    public static int TotalAmmo = 30;

    private static int maxTotalAmmo = 30;
    public static int MaxTotalAmmo
    {
        get => maxTotalAmmo + UpgradeManager.Instance.LargerPouch;
    }

    private static float meleeBuffer = 0.8f;
    public static float MeleeBuffer
    {
        get => Math.Max(0.1f, meleeBuffer - UpgradeManager.Instance.FasterSwinging);
    }
    private static float gunBuffer = 0.5f;
    public static float GunBuffer
    {
        get => gunBuffer;
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
        PlayerManager.Instance.HasUsedStamina(amount);
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

    public static void PactOfBlood()
    {
        bonusHealBuffer = 1;
        maxHealthBonus = 50;
    }

    public static void PactOfService()
    {
        bonusStamBuffer = 0.3f;
        bonusMaxStamina = 50;
    }

    public static void PactOfAgility()
    {
        speedBonus = 3;
    }


    #endregion
}
