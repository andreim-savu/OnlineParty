using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRacePerkPowerWalker : PetRacePerk
{
    public override void InitPerk(PetRacePet pet, int level)
    {
        _pet = pet;
        _level = level;
        switch (_level)
        {
            case 1:
                _pet.movementSpeed += 0.2f;
                _pet.runMultiplier -= 0.05f;
                break;
            case 2:
                _pet.movementSpeed += 0.4f;
                _pet.runMultiplier -= 0.1f;
                break;
            case 3:
                _pet.movementSpeed += 0.6f;
                _pet.runMultiplier -= 0.15f;
                break;
            case 4:
                _pet.movementSpeed += 0.8f;
                _pet.runMultiplier -= 0.2f;
                break;
            case 5:
                _pet.movementSpeed += 1.0f;
                _pet.runMultiplier -= 0.25f;
                break;
        }
    }
}
