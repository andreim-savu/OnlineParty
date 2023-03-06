using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRacePerkStartStamina : PetRacePerk
{
    public override void InitPerk(PetRacePet pet, int level)
    {
        _pet = pet;
        _level = level;
        _pet.currentStamina += _level * 10;
    }
}
