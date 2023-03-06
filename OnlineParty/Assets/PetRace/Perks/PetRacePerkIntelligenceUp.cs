using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRacePerkIntelligenceUp : PetRacePerk
{
    public override void InitPerk(PetRacePet pet, int level)
    {
        _pet = pet;
        _level = level;
        switch (_level)
        {
            case 1:
                _pet.intelligence += 3;
                break;
            case 2:
                _pet.intelligence += 6;
                break;
            case 3:
                _pet.intelligence += 9;
                break;
            case 4:
                _pet.intelligence += 12;
                break;
            case 5:
                _pet.intelligence += 15;
                break;
        }
    }
}
