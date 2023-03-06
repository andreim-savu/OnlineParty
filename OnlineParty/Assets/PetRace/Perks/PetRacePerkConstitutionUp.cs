using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRacePerkConstitutionUp : PetRacePerk
{
    public override void InitPerk(PetRacePet pet, int level)
    {
        _pet = pet;
        _level = level;
        switch (_level)
        {
            case 1:
                _pet.constitution += 3;
                break;
            case 2:
                _pet.constitution += 6;
                break;
            case 3:
                _pet.constitution += 9;
                break;
            case 4:
                _pet.constitution += 12;
                break;
            case 5:
                _pet.constitution += 15;
                break;
        }
    }
}
