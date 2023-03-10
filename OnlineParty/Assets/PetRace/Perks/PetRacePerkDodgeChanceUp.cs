using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRacePerkDodgeChanceUp : PetRacePerk
{
    public override void InitPerk(PetRacePet pet, int level)
    {
        _pet = pet;
        _level = level;
        switch (_level)
        {
            case 1:
                _pet.obstacleAvoidChance += 5;
                break;
            case 2:
                _pet.obstacleAvoidChance += 10;
                break;
            case 3:
                _pet.obstacleAvoidChance += 15;
                break;
            case 4:
                _pet.obstacleAvoidChance += 20;
                break;
            case 5:
                _pet.obstacleAvoidChance += 25;
                break;
        }
    }
}
