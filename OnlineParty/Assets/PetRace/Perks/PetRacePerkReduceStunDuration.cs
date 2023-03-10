using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRacePerkReduceStunDuration : PetRacePerk
{
    public override void InitPerk(PetRacePet pet, int level)
    {
        _pet = pet;
        _level = level;
        _pet.maxStunDuration -= _level switch
        {
            1 => 0.1f,
            2 => 0.2f,
            3 => 0.3f,
            4 => 0.4f,
            5 => 0.5f
        };
    }
}
