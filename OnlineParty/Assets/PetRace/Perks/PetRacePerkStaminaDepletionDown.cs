using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRacePerkStaminaDepletionDown : PetRacePerk
{
    public override void InitPerk(PetRacePet pet, int level)
    {
        _pet = pet;
        _level = level;
        switch (_level)
        {
            case 1:
                _pet.staminaConsumption -= 1.0f;
                break;
            case 2:
                _pet.staminaConsumption -= 2.0f;
                break;
            case 3:
                _pet.staminaConsumption -= 3.0f;
                break;
            case 4:
                _pet.staminaConsumption -= 4.0f;
                break;
            case 5:
                _pet.staminaConsumption -= 5.0f;
                break;
        }
    }
}
