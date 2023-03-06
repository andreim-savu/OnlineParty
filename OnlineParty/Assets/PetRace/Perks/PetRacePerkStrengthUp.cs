using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRacePerkStrengthUp : PetRacePerk
{
    public override void InitPerk(PetRacePet pet, int level)
    {
        _pet = pet;
        _level = level;
        switch (_level)
        {
            case 1:
                _pet.strength += 3;
                break;
            case 2:
                _pet.strength += 6;
                break;
            case 3:
                _pet.strength += 9;
                break;
            case 4:
                _pet.strength += 12;
                break;
            case 5:
                _pet.strength += 15;
                break;
        }
    }
}
