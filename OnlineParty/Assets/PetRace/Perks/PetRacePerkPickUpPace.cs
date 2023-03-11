using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRacePerkPickUpPace : PetRacePerk
{
    float modifier;
    public override void InitPerk(PetRacePet pet, int level)
    {
        _pet = pet;
        _level = level;
        switch (_level)
        {
            case 1:
                modifier = 0.05f;
                break;
            case 2:
                modifier = 0.1f;
                break;
            case 3:
                modifier = 0.15f;
                break;
            case 4:
                modifier = 0.2f;
                break;
            case 5:
                modifier = 0.25f;
                break;
        }

        _pet.onStopRunning.AddListener(Active);
    }

    void Active()
    {
        _pet.runMultiplier += modifier;
    }
}
