using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRacePerkAgilityUp : PetRacePerk
{
    public override void InitPerk(PetRacePet pet, int level)
    {
        _pet = pet;
        _level = level;
        switch (_level)
        {
            case 1:
                _pet.agility += 3;
                break;
            case 2:
                _pet.agility += 6;
                break;
            case 3:
                _pet.agility += 9;
                break;
            case 4:
                _pet.agility += 12;
                break;
            case 5:
                _pet.agility += 15;
                break;
        }
    }
}
