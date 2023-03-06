using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRacePerkStatMotivation : PetRacePerk
{
    public override void InitPerk(PetRacePet pet, int level)
    {
        _pet = pet;
        _level = level;
        _pet.onPerksAdded.AddListener(Active);
    }

    void Active()
    {
        for (int i = 0; i < _level; i++)
        {
            int highestStat = _pet.agility;
            int lowestStat = _pet.agility;

            if (_pet.strength > highestStat) { highestStat = _pet.strength; }
            if (_pet.intelligence > highestStat) { highestStat = _pet.intelligence; }
            if (_pet.constitution > highestStat) { highestStat = _pet.constitution; }
            if (_pet.endurance > highestStat) { highestStat = _pet.endurance; }
            if (_pet.dexterity > highestStat) { highestStat = _pet.dexterity; }

            if (_pet.strength < lowestStat) { lowestStat = _pet.strength; }
            if (_pet.intelligence < lowestStat) { lowestStat = _pet.intelligence; }
            if (_pet.constitution < lowestStat) { lowestStat = _pet.constitution; }
            if (_pet.endurance < lowestStat) { lowestStat = _pet.endurance; }
            if (_pet.dexterity < lowestStat) { lowestStat = _pet.dexterity; }

            if (_pet.strength == lowestStat) { _pet.strength = highestStat; }
            if (_pet.intelligence == lowestStat) { _pet.intelligence = highestStat; }
            if (_pet.constitution == lowestStat) { _pet.constitution = highestStat; }
            if (_pet.endurance == lowestStat) { _pet.endurance = highestStat; }
            if (_pet.dexterity == lowestStat) { _pet.dexterity = highestStat; }
        }
    }
}
