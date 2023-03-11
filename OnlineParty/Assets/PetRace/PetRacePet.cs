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
    [SerializeField] Transform _modelTransform;

    public float runMultiplier = 1.25f;
    public float maxStamina = 100.0f;
    public float maxStunDuration = 1.0f;
    public float waterMoveModifier = 0.5f;
    public float sandMoveModifier = 0.5f;
    public float currentStamina = 0;
    public float stunDuration = 0.0f;

    public float movementSpeed;
    public float obstacleAvoidChance;
    public float staminaRegeneration;
    public float staminaConsumption;

    public float loudRunnerModifier = 1;

    void CalculatePetStats()
    {
        onStatsCalculated.Invoke();
    }

    public float raceProgress = 0f;


    public PetRaceTrack _track;
     
    public bool isRacing = false;

    public bool running = false;

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

    public string id;

    public void InitPet(IPetRacePet petData)
    {
        movementSpeed = petData.movementSpeed;
        obstacleAvoidChance = petData.dodge;
        staminaRegeneration = petData.regen;
        staminaConsumption = petData.depletion;

        Instantiate(PetRacePetModels.Instance.GetModel(petData.modelNo), _modelTransform);
        nameTM.text = petData.name;
        id = petData.id;

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
                if (_isOnSand || _isOnWater)
                {
                    transform.localPosition += new Vector3(0, 0.2f, 0);
                }
                _isOnSand = false;
                _isOnWater = false;
            }
            if (tileType == '1')
            {
                if (!_isOnSand && !_isOnWater)
                {
                    transform.localPosition -= new Vector3(0, 0.2f, 0);
                }
                _isOnSand = true;
                _isOnWater = false;
            }
            if (tileType == '2')
            {
                if (!_isOnSand && !_isOnWater)
                {
                    transform.localPosition -= new Vector3(0, 0.2f, 0);
                }
                _isOnSand = false;
                _isOnWater = true;
            }
            if (tileType == '3')
            {
                if (_isOnSand || _isOnWater)
                {
                    transform.localPosition += new Vector3(0, 0.2f, 0);
                }
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
                if (_isOnSand || _isOnWater)
                {
                    transform.localPosition += new Vector3(0, 0.2f, 0);
                }
                isRacing = false;
                raceProgress = 1;
                PetRaceCanvas.Instance.HandleFinishRace(this);
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
