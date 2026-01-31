using System.Collections.Generic;
using UnityEngine;

public enum SigilType
{
    Blood, 
    Service,
    Agility,
    Time,
    Binding
}

public class SigilManager : MonoBehaviour
{
    public static SigilManager Instance {  get; private set; }

    private HashSet<SigilType> currentSigils = new HashSet<SigilType>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;

    }

    private SigilType currentSigil;

    [SerializeField]
    private int bloodCost = 150, serviceCost = 70, agilityCost = 1000, timeCost = 125, bindingCost = 250;

    public bool CanPurchaseSigil(SigilType sigil)
    {
        currentSigil = sigil;
        if(SigilCost() <= PlayerStats.Souls && !currentSigils.Contains(currentSigil))
        {
            return true;
        }

        Debug.Log("Ya cant get this sigil chief");
        return false;
    }

    private int SigilCost()
    {
        switch(currentSigil)
        {
            case SigilType.Blood:
                return bloodCost;
            case SigilType.Service:
                return serviceCost;
            case SigilType.Agility:
                return agilityCost;
            case SigilType.Time:
                return timeCost;
            case SigilType.Binding:
                return bindingCost;
        }

        return 1000000;
    }



    public void PurchaseSigil(SigilType sigil)
    {
        Debug.Log("Yo yo yo we buyin sigils");
        if(!currentSigils.Contains(sigil))
        {
            PlayerStats.SpendSouls(SigilCost());
            currentSigils.Add(sigil);
            ActivateSigil(sigil);

        }
    }

    private void ActivateSigil(SigilType newSigil)
    {
        switch (currentSigil)
        {
            case SigilType.Blood:
                PlayerStats.PactOfBlood();
                break;
            case SigilType.Service:
                PlayerStats.PactOfService();
                break;
            case SigilType.Agility:
                PlayerStats.PactOfAgility();
                break;
            case SigilType.Time:
                MaskManager.Instance.PactOfTime();
                break;
            case SigilType.Binding:
                //TODO: Idk how tf they expect me to do this
                break;
        }

        Debug.Log("They do be activatin they sigils");
    }
}
