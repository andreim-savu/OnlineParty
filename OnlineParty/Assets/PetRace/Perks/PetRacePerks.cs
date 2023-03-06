using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRacePerks : MonoBehaviour
{
    public static PetRacePerks Instance;
    [SerializeField] Dictionary<string, string> perksDict = new Dictionary<string, string>();

    private void Awake()
    {
        if (Instance != null) { Destroy(this); }
        else { Instance = this; }
    }

    private void Start()
    {
        perksDict.Add("0", "PetRacePerkAgilityUp");
        perksDict.Add("1", "PetRacePerkStrengthUp");
        perksDict.Add("2", "PetRacePerkIntelligenceUp");
        perksDict.Add("3", "PetRacePerkConstitutionUp");
        perksDict.Add("4", "PetRacePerkEnduranceUp");
        perksDict.Add("5", "PetRacePerkDexterityUp");
        perksDict.Add("6", "PetRacePerkAllStatsUp");
        perksDict.Add("7", "PetRacePerkAvoidObstacle");
        perksDict.Add("8", "PetRacePerkRunBurst"); 
        perksDict.Add("9", "PetRacePerkEnergySiphon");
        perksDict.Add("10", "PetRacePerkStunShare"); 
        perksDict.Add("11", "PetRacePerkStartStamina");
        perksDict.Add("12", "PetRacePerkFastRefresh"); 
        perksDict.Add("13", "PetRacePerkSlowStart");
        perksDict.Add("14", "PetRacePerkPositiveFrustration");
        perksDict.Add("15", "PetRacePerkLoudRunner");
        perksDict.Add("16", "PetRacePerkFastFinish");
        perksDict.Add("17", "PetRacePerkReduceStunDuration");
        perksDict.Add("18", "PetRacePerkWaterSpecialist");
        perksDict.Add("19", "PetRacePerkSandSpecialist");
        perksDict.Add("20", "PetRacePerkMotivatedWalker");
        perksDict.Add("21", "PetRacePerkStatMotivation");
    }

    public string GetPerk(string index)
    {
        perksDict.TryGetValue(index, out string perk);
        return perk;
    }
}
