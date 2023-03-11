using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRacePerkRunSpeedUp : PetRacePerk
{
    public override void InitPerk(PetRacePet pet, int level)
    {
        _pet = pet;
        _level = level;
        switch (_level)
        {
            case 1:
                _pet.runMultiplier += 0.05f;
                break;
            case 2:
                _pet.runMultiplier += 0.1f;
                break;
            case 3:
                _pet.runMultiplier += 0.15f;
                break;
            case 4:
                _pet.runMultiplier += 0.2f;
                break;
            case 5:
                _pet.runMultiplier += 0.25f;
                break;
        }
    }
}
