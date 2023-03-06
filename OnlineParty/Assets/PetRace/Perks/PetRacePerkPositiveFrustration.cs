using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRacePerkPositiveFrustration : PetRacePerk
{
    float movementSpeedModifier;
    public override void InitPerk(PetRacePet pet, int level)
    {
        _pet = pet;
        _level = level;
        switch (_level)
        {
            case 1:
                movementSpeedModifier = 0.1f;
                break;
            case 2:
                movementSpeedModifier = 0.2f;
                break;
            case 3:
                movementSpeedModifier = 0.3f;
                break;
            case 4:
                movementSpeedModifier = 0.4f;
                break;
            case 5:
                movementSpeedModifier = 0.5f;
                break;
        }
        _pet.onGetStunned.AddListener(Active);
    }

    void Active()
    {
        _pet.movementSpeed += movementSpeedModifier;
    }
}
