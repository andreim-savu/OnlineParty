using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRacePerkFastRefresh : PetRacePerk
{
    public override void InitPerk(PetRacePet pet, int level)
    {
        _pet = pet;
        _level = level;
        _pet.onStopRunning.AddListener(Active);
    }

    void Active()
    {
        _pet.currentStamina += 10 * _level;
    }
}
