using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    //Handles Attacking
    //Static shop can upgrade what you have 
    //Call of duty esqe perks around the map, max 3 equipped, set locations(maybe randomized later)

    
    public static PlayerManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }


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
    private Coroutine ReloadCoroutine;

    private bool gunEquiped = false;
    private bool gunOnCD, swordOnCD;
    [SerializeField]
    private GameObject bulletPrefab;

    private void EquipGun(InputAction.CallbackContext context)
    {
        if(ReloadCoroutine != null)
            StopCoroutine(ReloadCoroutine);

        gunEquiped = true;
    }

    private void UnequipGun(InputAction.CallbackContext context)
    {
        gunEquiped = false;

        if (PlayerStats.CurrentAmmo < PlayerStats.MaxGunAmmo)
            ReloadCoroutine = StartCoroutine(Reload());
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if(!CanAttack())
            return;

        
        if(!gunEquiped)
        {
            //Hit anything in range
            AttackDetection.Instance.Attack();

            StartCoroutine(MeleeCD());
        }
        else
        {   //Fire projectile

            //TODO: Projectile is slightly off center, fix

            //Get screen center 
            Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

            //Get mouse offset
            Vector2 mouseOffset = Mouse.current.position.ReadValue() - screenCenter;

            //Convert to normalized direction
            Vector3 direction = new Vector3(mouseOffset.x, 0f, mouseOffset.y).normalized;
            //Get bullet direction
            Quaternion bulletDir = Quaternion.LookRotation(direction, Vector3.up);
            //Create bullet
            Instantiate(bulletPrefab, transform.position, bulletDir);

            Debug.Log("Gun should shoot now");
            PlayerStats.ShootGun();
            StartCoroutine(GunCD());
        }
    }

    private bool CanAttack()
    {
        //Check for gun equiped and CD on gun or sword
        //Also need to check if dodging and other movement shit
        if(gunEquiped)
        {
            if(PlayerStats.CurrentAmmo > 0 && !gunOnCD)
            {
                Debug.Log("can bang bang attack :)");
                return true;
            }
        }
        else
        {
            //Check melee CD
            if(!swordOnCD)
            {
                Debug.Log("We can meelee");
                return true;
            }
        }

        Debug.Log("cant attack :(");
            return false;
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(1f);

        while (PlayerStats.CurrentAmmo < PlayerStats.MaxGunAmmo)
        {
           PlayerStats.ReloadGun();

            yield return new WaitForSeconds(0.5f);
        }

        yield return null;
    }

    private IEnumerator MeleeCD()
    {
        swordOnCD = true;

        yield return new WaitForSeconds(PlayerStats.meleeBuffer);

        swordOnCD = false;
    }

    private IEnumerator GunCD()
    {
        gunOnCD = true;

        yield return new WaitForSeconds(PlayerStats.gunBuffer);

        gunOnCD = false;
    }
    #endregion

    #region Damage/Healing
    private Coroutine HealingStageCoroutine;


    public void HasTakenDamage(int damage)
    {
        if(PlayerStats.RemoveHealth(damage))
        {
            GameOver();
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
            //Debug.Log($"We are at {PlayerStats.Health} health out of {PlayerStats.MaxHealth}. With {currentHealTimer} seconds elapsed");
            PlayerStats.AddHealth(1);


            currentHealTimer += healBuffer;
            yield return new WaitForSeconds(healBuffer);
        }

        yield return null;
    }

    private void GameOver()
    {
        //TODO: display game over ui

        //Time scale = 0
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
