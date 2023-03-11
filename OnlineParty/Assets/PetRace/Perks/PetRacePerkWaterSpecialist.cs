using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRacePerkWaterSpecialist : PetRacePerk
{
    float modifier;
    public override void InitPerk(PetRacePet pet, int level)
    {
        _pet = pet;
        _level = level;
        switch (_level)
        {
            case 1:
                modifier = 0.1f;
                break;
            case 2:
                modifier = 0.2f;
                break;
            case 3:
                modifier = 0.3f;
                break;
            case 4:
                modifier = 0.4f;
                break;
            case 5:
                modifier = 0.5f;
                break;
        }

        _pet.waterMoveModifier += modifier;
    }
}
