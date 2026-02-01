using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;

        buyEfficientSwing.onClick.AddListener(() => BuySwordUpgrade(0));
        buyFasterSwing.onClick.AddListener(() => BuySwordUpgrade(1));
        buyHarderSwing.onClick.AddListener(() => BuySwordUpgrade(2));

        buyHarderBullet.onClick.AddListener(() => BuyGunUpgrade(0));
        buyLargerChamber.onClick.AddListener(() => BuyGunUpgrade(1));
        buyLargerPouch.onClick.AddListener(() => BuyGunUpgrade(2));
        buyNimbleFingers.onClick.AddListener(() => BuyGunUpgrade(3));
        buySpeedWalker.onClick.AddListener(() => BuyGunUpgrade(4));

        gunCostTxt.text = $"Gun Cost: {GunUpgradeCost}";
        swordCostTxt.text = $"Sword Cost: {SwordUpgradeCost}";
    }

    #region UI
    [SerializeField]
    private Button buyEfficientSwing, buyFasterSwing,buyHarderSwing;

    [SerializeField]
    private Button buyHarderBullet, buyLargerChamber, buyLargerPouch, buyNimbleFingers, buySpeedWalker;

    [SerializeField]
    private TextMeshProUGUI gunCostTxt, swordCostTxt;

    private void BuyGunUpgrade(int gunId)
    {
        if(CanBuyUpgrade(GunUpgradeCost))
        {
            //Play weaponUpgrade sound
            AudioManager.instance.PlayOneShot(FMODEvents.instance.weaponUpgrade, this.transform.position);
            PlayerStats.SpendSouls(GunUpgradeCost);
            GunUpgradesPurchased++;

            switch (gunId)
            {
                case 0:
                    harderBulletAmount++;
                    break;
                case 1:
                    largerChamberAmount++;
                    break;
                case 2:
                    largerPouchAmount++;
                    break;
                case 3:
                    nimbleFingersAmount++;
                    break;
                case 4:
                    speedWalkerAmount++;
                    break;

                default:
                    goto case 0;
            }

            gunCostTxt.text = $"Gun Cost: {GunUpgradeCost}";
        }
    }

    private void BuySwordUpgrade(int swordId)
    {
        if(CanBuyUpgrade(SwordUpgradeCost))
        {
            //Play weaponUpgrade sound
            AudioManager.instance.PlayOneShot(FMODEvents.instance.weaponUpgrade, this.transform.position);
            PlayerStats.SpendSouls(SwordUpgradeCost);
            SwordUpgradesPurchased++;

            switch(swordId)
            {
                case 0:
                    efficientSwingAmount++;
                    break;
                case 1:
                    fasterSwingingAmount++;
                    break;
                case 2:
                    harderSwingsAmount++;
                    break;

                default:
                    goto case 0;
            }

            swordCostTxt.text = $"Sword Cost: {SwordUpgradeCost}";
        }
    }

    private bool CanBuyUpgrade(int upgradeCost)
    {
        if(upgradeCost <= PlayerStats.Souls)
        {
            return true;
        }
        return false;
    }

    #endregion

    #region Cost Scaling
    private int baseGunPrice = 25;
    private int GunCostMultiplier  = 8;
    private int GunUpgradesPurchased = 0;
    private int GunUpgradeCost
    {
        get => baseGunPrice + (GunCostMultiplier * GunUpgradesPurchased);
    }

    private int baseSwordPrice = 25;
    private int SwordCostMultiplier = 1;
    private int SwordUpgradesPurchased = 0;
    private int SwordUpgradeCost
    {
        get => baseSwordPrice + (SwordCostMultiplier * SwordUpgradesPurchased);
    }

    #endregion

    #region Sword Upgrades
    /// <summary>
    /// How much stamina your sword uses
    /// </summary>
    public int EfficientSwing
    {
        get => 2 * efficientSwingAmount;
    }
    private int efficientSwingAmount = 0;

    /// <summary>
    /// How quickly the cd on sword attacks comes back
    /// </summary>
    public float FasterSwinging
    {
        get => 0.02f * fasterSwingingAmount;
    }
    private int fasterSwingingAmount = 0;

    /// <summary>
    /// How much damage you do with the sword
    /// </summary>
    public int HarderSwings
    {
        get => 8 * harderSwingsAmount;
    }
    private int harderSwingsAmount = 0;

    #endregion

    #region Gun Upgrades
    /// <summary>
    /// How much damage with gun
    /// </summary>
    ///  
    public int HarderBullet
    {
        get => 5 * harderBulletAmount;
    }
    private int harderBulletAmount = 0;
   

    /// <summary>
    /// How many bullets can be in the gun at once
    /// </summary>
    public int LargerChamber
    {
        get => largerChamberAmount;
    }
    private int largerChamberAmount = 0;

    /// <summary>
    /// How many bullets you can hold total(reserve ammo)
    /// </summary>
    public int LargerPouch
    {
        get => 10* largerPouchAmount;
    }
    private int largerPouchAmount = 0;

    /// <summary>
    /// How quickly you can reload
    /// </summary>
    public float NimbleFingers
    {
        get => 0.2f * nimbleFingersAmount;
    }
    private int nimbleFingersAmount = 0;

    /// <summary>
    /// Reduces the penalty for moving with gun out
    /// </summary>
    public float SpeedWalker
    {
        get => 0.1f * speedWalkerAmount;
    }
    private int speedWalkerAmount = 0;
    #endregion

}
