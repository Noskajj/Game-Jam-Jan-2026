using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Ambience")]
    [field: SerializeField] public EventReference flowingWater { get; private set; }
    [field: SerializeField] public EventReference fireCrackling { get; private set; }
    [field: SerializeField] public EventReference scaryAmbience { get; private set; }
    [field: SerializeField] public EventReference scaryAmbience2 { get; private set; }

    [field: Header("Music")]
    [field: SerializeField] public EventReference menuTheme { get; private set; }
    [field: SerializeField] public EventReference backgroundMusic { get; private set; }
    [field: SerializeField] public EventReference transitionToBossFight { get; private set; }
    [field: SerializeField] public EventReference phase1OfBossFight { get; private set; }
    [field: SerializeField] public EventReference phase2OfBossFight { get; private set; }
    [field: SerializeField] public EventReference bossFightEnding { get; private set; }

    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference playerFootstepsGravel { get; private set; }
    [field: SerializeField] public EventReference playerFootstepsWood { get; private set; }
    [field: SerializeField] public EventReference playerFootstepsStone { get; private set; }
    [field: SerializeField] public EventReference playerDash { get; private set; }
    [field: SerializeField] public EventReference playerHurt { get; private set; }
    [field: SerializeField] public EventReference maskCollected { get; private set; }
    [field: SerializeField] public EventReference magicalInfusion { get; private set; }
    [field: SerializeField] public EventReference weaponUpgrade { get; private set; }
    [field: SerializeField] public EventReference swordSlash { get; private set; }
    [field: SerializeField] public EventReference pistolFire { get; private set; }

    [field: Header("Enemy SFX")]
    [field: SerializeField] public EventReference enemyFootsteps { get; private set; }
    [field: SerializeField] public EventReference enemyMagicAttack { get; private set; }

    [field: Header("Monster SFX")]
    [field: SerializeField] public EventReference knifeThrow { get; private set; }
    [field: SerializeField] public EventReference monsterMelee { get; private set; }
    [field: SerializeField] public EventReference monsterHurt { get; private set; }

    [field: Header("Final Boss SFX")]
    [field: SerializeField] public EventReference uncleDash { get; private set; }
    [field: SerializeField] public EventReference uncleHurt { get; private set; }
    [field: SerializeField] public EventReference uncleSlam { get; private set; }
    [field: SerializeField] public EventReference uncleLaughter { get; private set; }

    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events instance in the scene.");
        }
        instance = this;
    }
}
