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
                movementSpeedModifier = 0.05f;
                break;
            case 2:
                movementSpeedModifier = 0.01f;
                break;
            case 3:
                movementSpeedModifier = 0.15f;
                break;
            case 4:
                movementSpeedModifier = 0.2f;
                break;
            case 5:
                movementSpeedModifier = 0.25f;
                break;
        }
        _pet.onGetStunned.AddListener(Active);
    }

    void Active()
    {
        _pet.movementSpeed += movementSpeedModifier;
    }
}
