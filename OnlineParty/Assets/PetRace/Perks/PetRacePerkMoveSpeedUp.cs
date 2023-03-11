using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRacePerkMoveSpeedUp : PetRacePerk
{
    public override void InitPerk(PetRacePet pet, int level)
    {
        _pet = pet;
        _level = level;
        switch (_level)
        {
            case 1:
                _pet.movementSpeed += 0.1f;
                break;
            case 2:
                _pet.movementSpeed += 0.2f;
                break;
            case 3:
                _pet.movementSpeed += 0.3f;
                break;
            case 4:
                _pet.movementSpeed += 0.4f;
                break;
            case 5:
                _pet.movementSpeed += 0.5f;
                break;
        }
    }
}
