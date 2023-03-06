using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRacePerkDexterityUp : PetRacePerk
{
    public override void InitPerk(PetRacePet pet, int level)
    {
        _pet = pet;
        _level = level;
        switch (_level)
        {
            case 1:
                _pet.dexterity += 3;
                break;
            case 2:
                _pet.dexterity += 6;
                break;
            case 3:
                _pet.dexterity += 9;
                break;
            case 4:
                _pet.dexterity += 12;
                break;
            case 5:
                _pet.dexterity += 15;
                break;
        }
    }
}
