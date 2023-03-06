using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRacePerkAvoidObstacle : PetRacePerk
{
    int charges;
    public override void InitPerk(PetRacePet pet, int level)
    {
        _pet = pet;
        _level = level;
        _pet.onGetStunned.AddListener(RemoveStun);

        charges = _level;
    }

    void RemoveStun()
    {
        if (charges > 0)
        {
            _pet.stunDuration = 0;
            charges--;
        }
    }
}
