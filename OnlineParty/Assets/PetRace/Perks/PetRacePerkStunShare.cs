using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRacePerkStunShare : PetRacePerk
{
    public int charges;
    float durationModifier;
    public override void InitPerk(PetRacePet pet, int level)
    {
        _pet = pet;
        _level = level;
        switch (_level)
        {
            case 1:
                durationModifier = 1;
                charges = 1;
                break;
            case 2:
                durationModifier = 1;
                charges = 2;
                break;
            case 3:
                durationModifier = 1.5f;
                charges = 2;
                break;
            case 4:
                durationModifier = 1.5f;
                charges = 3;
                break;
            case 5:
                durationModifier = 2;
                charges = 3;
                break;
        }

        _pet.onGetStunned.AddListener(Active);
    }

    void Active()
    {
        if (charges == 0) { return; }
        foreach (PetRacePet pet in PetRaceCanvas.Instance._petObjects)
        {
            if (pet == _pet) { continue; }
            if (pet.stunDuration > 0) { continue; }
            pet.GetStunned(pet.maxStunDuration * durationModifier);
        }
        charges--;
    }
}
