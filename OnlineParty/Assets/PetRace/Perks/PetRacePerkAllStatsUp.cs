using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRacePerkAllStatsUp : PetRacePerk
{
    public override void InitPerk(PetRacePet pet, int level)
    {
        _pet = pet;
        _level = level;

        _pet.agility += _level;
        _pet.strength += _level;
        _pet.intelligence += _level;
        _pet.constitution += _level;
        _pet.endurance += _level;
        _pet.dexterity += _level;
    }
}
