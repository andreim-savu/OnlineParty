using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRacePerkStaminaRegenUp : PetRacePerk
{
    public override void InitPerk(PetRacePet pet, int level)
    {
        _pet = pet;
        _level = level;
        switch (_level)
        {
            case 1:
                _pet.staminaRegeneration += 1.5f;
                break;
            case 2:
                _pet.staminaRegeneration += 3.0f;
                break;
            case 3:
                _pet.staminaRegeneration += 4.5f;
                break;
            case 4:
                _pet.staminaRegeneration += 6.0f;
                break;
            case 5:
                _pet.staminaRegeneration += 7.5f;
                break;
        }
    }
}
