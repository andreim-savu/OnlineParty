using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class PetRacePet : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameTM;
    [SerializeField] Slider staminaSlider;

    public float baseMoveSpeed = 2.0f;
    public float baseRunMultiplier = 1.25f;
    public float baseObstacleAvoidChance = 5.0f;
    public float baseStaminaRegeneration = 15.0f;
    public float baseStaminaConsumption = 25.0f;
    public float baseMaxStamina = 100.0f;
    public float baseMaxStunDuration = 1.0f;
    public float baseWaterMoveModifier = 0.5f;
    public float baseSandMoveModifier = 0.5f;

    public float movementSpeed;
    public float runMultiplier;
    float obstacleAvoidChance;
    public float currentStamina = 0;
    public float maxStamina;
    float staminaRegeneration;
    float staminaConsumption;
    public float stunDuration = 0.0f;
    public float maxStunDuration;
    float waterMoveModifier;
    float sandMoveModifier;

    public float loudRunnerModifier = 1;

    void CalculatePetStats()
    {
        movementSpeed = baseMoveSpeed + agility * 0.1f;
        runMultiplier = baseRunMultiplier + strength * 0.02f;
        obstacleAvoidChance = baseObstacleAvoidChance + intelligence * 0.8f;
        maxStamina = baseMaxStamina;
        staminaRegeneration = baseStaminaRegeneration + constitution * 0.5f;
        staminaConsumption = baseStaminaConsumption - endurance * 0.05f;
        maxStunDuration = baseMaxStunDuration - dexterity * 0.005f;
        waterMoveModifier = baseWaterMoveModifier + dexterity * 0.01f;
        sandMoveModifier = baseSandMoveModifier + dexterity * 0.01f;
        onStatsCalculated.Invoke();
    }

    public int agility;
    public int strength;
    public int intelligence;
    public int constitution;
    public int endurance;
    public int dexterity;

    public float raceProgress = 0f;


    public PetRaceTrack _track;
     
    public bool isRacing = false;

    bool running = false;

    bool _isOnWater = false;
    bool _isOnSand = false;

    string currentTile = "0";

    public UnityEvent onGetStunned = new UnityEvent();
    public UnityEvent onStartRunning = new UnityEvent();
    public UnityEvent onStopRunning = new UnityEvent();
    public UnityEvent onPerksAdded = new UnityEvent();
    public UnityEvent onStatsCalculated = new UnityEvent();
    public UnityEvent onTrackPlaced = new UnityEvent();

    public float extraRunFlatSpeed = 0;

    public void InitPet(IPetRacePet petData)
    {
        agility = petData.agility;
        strength = petData.strength;
        intelligence = petData.intelligence;
        constitution = petData.constitution;
        endurance = petData.endurance;
        dexterity = petData.dexterity;

        nameTM.text = petData.name;

        foreach (IPetRacePerk perk in petData.perks)
        {
            PetRacePerk newPerk = (PetRacePerk)gameObject.AddComponent(Type.GetType(PetRacePerks.Instance.GetPerk(perk.id)));
            newPerk.InitPerk(this, perk.level);
        }
        onPerksAdded.Invoke();

        CalculatePetStats();
    }

    public void SetTrack(PetRaceTrack track)
    {
        transform.localPosition = new Vector3(0, 1, 0);
        _track = track;
        onTrackPlaced.Invoke();
        _track.InitTrack();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRacing) { return; }

        raceProgress = transform.localPosition.z / (_track.trackLength - 1);

        if (stunDuration > 0)
        {
            stunDuration -= Time.deltaTime;
            return;
        }

        CheckTile();

        if (!running) { StaminaRegen(); }

        if (running) { Run(); }
        else { Walk(); }
        staminaSlider.value = currentStamina / maxStamina;
    }

    /*
        0 - Normal
        1 - Sand
        2 - Water
        3 - Obstacle
    */
    void StaminaRegen()
    {
        if (currentStamina < maxStamina) { currentStamina += staminaRegeneration * Time.deltaTime; }
        if (currentStamina >= maxStamina)
        {
            currentStamina = maxStamina;
            running = true;
            onStartRunning.Invoke();
        }
    }

    void CheckTile()
    {
        string tilePos = transform.localPosition.z.ToString("0");
        if (tilePos != currentTile)
        {
            currentTile = tilePos;
            char tileType = _track.GetTile(tilePos);
            if (tileType == '0')
            {
                _isOnSand = false;
                _isOnWater = false;
            }
            if (tileType == '1')
            {
                _isOnSand = true;
                _isOnWater = false;
            }
            if (tileType == '2')
            {
                _isOnSand = false;
                _isOnWater = true;
            }
            if (tileType == '3')
            {
                _isOnSand = false;
                _isOnWater = false;
                if (UnityEngine.Random.Range(0, 101) > obstacleAvoidChance)
                {
                    GetStunned(maxStunDuration);
                }
                return;
            }
            if (tileType == '4')
            {
                isRacing = false;
                raceProgress = 1;
                PetRaceCanvas.Instance.HandleFinishRace();
                return;
            }
        }
    }

    void Walk()
    {
        transform.Translate(Vector3.forward * (movementSpeed * (_isOnWater ? waterMoveModifier : 1)) * loudRunnerModifier * Time.deltaTime);
    }

    void Run()
    {
        transform.Translate(Vector3.forward * ((movementSpeed * runMultiplier + extraRunFlatSpeed) * (_isOnSand ? sandMoveModifier : 1) * 1.5f) * loudRunnerModifier * Time.deltaTime);
        currentStamina -= staminaConsumption * Time.deltaTime;

        if (currentStamina <= 0)
        {
            currentStamina = 0;
            running = false;
            onStopRunning.Invoke();
        }
    }

    public void StartRacing()
    {
        isRacing = true;
    }

    public void GetStunned(float duration)
    {
        stunDuration = duration;
        onGetStunned.Invoke();
    }
}
