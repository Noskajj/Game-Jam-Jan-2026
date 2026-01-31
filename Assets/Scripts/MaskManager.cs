using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MaskManager : MonoBehaviour
{
    public static MaskManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;

    }

    private InputAction useMask1, useMask2, useMask3;
    private void Start()
    {
        useMask1 = InputSystem.actions.FindAction("Mask1Activate");
        useMask2 = InputSystem.actions.FindAction("Mask2Activate");
        useMask3 = InputSystem.actions.FindAction("Mask3Activate");

        useMask1.started += CheckForMask1;
        useMask2.started += CheckForMask2;
        useMask3.started += CheckForMask3;

    }


    [SerializeField]
    private int allMasks, masksCollected;

    float maskCDMultiplier = 1;

    public void PactOfTime()
    {
        maskCDMultiplier = 0.7f;
    }

    public void MaskCollected()
    {
        masksCollected++;
    }

    #region mask1
    private float mask1CD
    {
        get => 45f * maskCDMultiplier;
    } 
    private float mask1Duration = 5f;
    private bool mask1OnCD = false;
    public delegate void Mask1CDInvoked((float cooldown, float active) values);
    public Mask1CDInvoked Mask1Invoked;
    //Main mask, damage boost for 5 seconds, cd 45
    private void CheckForMask1(InputAction.CallbackContext context)
    {
        if(!mask1OnCD && masksCollected >=1)
        {
            //DO mask 1 effect
            StartCoroutine(Mask1Activated());
        }
    }

    private IEnumerator Mask1Activated()
    {
        //Boosts damage by 100%
        PlayerStats.meleeBonus = PlayerStats.MeleeDamage;
        PlayerStats.gunBonus = PlayerStats.GunDamage;
        mask1OnCD = true;

        Mask1Invoked?.Invoke((mask1CD, mask1Duration));
        yield return new WaitForSeconds(mask1Duration);

        
        
        yield return new WaitForSeconds(mask1CD);

        mask1OnCD = false;
    }

    #endregion

    #region mask2
    private float mask2CD
    {
        get => 12f * maskCDMultiplier;
    }
    private bool mask2OnCD = false;
    [SerializeField]
    private GameObject magicProjectilePrefab;
    public delegate void Mask2CDInvoked((float cooldown, float active) values);
    public Mask1CDInvoked Mask2Invoked;

    //Mask 2, spells projectile aoe, cd 12
    private void CheckForMask2(InputAction.CallbackContext context)
    {
        if (!mask2OnCD && masksCollected >= 2)
        {
            //DO mask 2 effect
            StartCoroutine(Mask2Activated());
        }
    }

    private IEnumerator Mask2Activated()
    {
        mask2OnCD = true;

        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        //Get mouse offset
        Vector2 mouseOffset = Mouse.current.position.ReadValue() - screenCenter;

        //Convert to normalized direction
        Vector3 direction = new Vector3(mouseOffset.x, 0f, mouseOffset.y).normalized;
        //Get bullet direction
        Quaternion bulletDir = Quaternion.LookRotation(direction, Vector3.up);
        //Create bullet
        Instantiate(magicProjectilePrefab, PlayerManager.Instance.transform.position, bulletDir);

        
        Mask2Invoked?.Invoke((mask2CD, 0f));
        yield return new WaitForSeconds(mask2CD);

        mask2OnCD = false;
    }
    #endregion

    #region mask3
    private float mask3CD
    {
        get => 120f * maskCDMultiplier;
    }
    private bool mask3OnCD = false;
    private float mask3Duration = 10f;
    public bool mask3IsActive = false;
    public delegate void Mask3CDInvoked((float cooldown, float active) values);
    public Mask1CDInvoked Mask3Invoked;

    public static event Action mask3Activated, mask3Deactivated;
    //Mask 3, AOE stun, cd 120
    private void CheckForMask3(InputAction.CallbackContext context)
    {
        if (!mask3OnCD && masksCollected >= 3)
        {
            //DO mask 3 effect
            StartCoroutine(Mask3Activated());
        }
    }

    private IEnumerator Mask3Activated()
    {
        mask3Activated?.Invoke();
        mask3IsActive = true;
        mask3OnCD = true;

        Mask3Invoked?.Invoke((mask3CD, mask3Duration));
        yield return new WaitForSeconds(mask3Duration);

        mask3IsActive = false;
        mask3Deactivated?.Invoke();

        
        
        yield return new WaitForSeconds(mask3CD);

        mask3OnCD = false;
    }
    #endregion
}
