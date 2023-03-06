using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRacePerkEnduranceUp : PetRacePerk
{
    public override void InitPerk(PetRacePet pet, int level)
    {
        _pet = pet;
        _level = level;
        switch (_level)
        {
            case 1:
                _pet.endurance += 3;
                break;
            case 2:
                _pet.endurance += 6;
                break;
            case 3:
                _pet.endurance += 9;
                break;
            case 4:
                _pet.endurance += 12;
                break;
            case 5:
                _pet.endurance += 15;
                break;
        }
    }
}
