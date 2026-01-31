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
    private SigilType currentSigil;

    [SerializeField]
    private int bloodCost = 20, serviceCost = 30, agilityCost = 40, timeCost = 50, bindingCost = 60;

    public bool CanPurchaseSigil(SigilType sigil)
    {
        currentSigil = sigil;
        if(SigilCost() <= PlayerStats.Souls)
        {
            return true;
        }

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
}
