using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRacePerkExcitedRunner : PetRacePerk
{
    float modifier;
    public override void InitPerk(PetRacePet pet, int level)
    {
        _pet = pet;
        _level = level;
        switch (_level)
        {
            case 1:
                modifier = 1.2f;
                break;
            case 2:
                modifier = 1.4f;
                break;
            case 3:
                modifier = 1.6f;
                break;
            case 4:
                modifier = 1.8f;
                break;
            case 5:
                modifier = 2.0f;
                break;
        }
        _pet.onPerksAdded.AddListener(Action);
    }

    void Action()
    {
        _pet.staminaRegeneration *= modifier;
        _pet.staminaConsumption *= modifier;
    }
}
