using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRacePerkEnergySiphon : PetRacePerk
{
    int staminaSteal;
    int charges;
    public override void InitPerk(PetRacePet pet, int level)
    {
        _pet = pet;
        _level = level;
        switch (_level)
        {
            case 1:
                staminaSteal = 5;
                charges = 1;
                break;
            case 2:
                staminaSteal = 5;
                charges = 2;
                break;
            case 3:
                staminaSteal = 10;
                charges = 2;
                break;
            case 4:
                staminaSteal = 10;
                charges = 3;
                break;
            case 5:
                staminaSteal = 15;
                charges = 3;
                break;
        }

        _pet.onStopRunning.AddListener(Active);
    }

    void Active()
    {
        if (charges == 0) { return; }
        foreach (PetRacePet pet in PetRaceCanvas.Instance._petObjects)
        {
            if (pet == _pet) { continue; }
            pet.currentStamina = Mathf.Max(0, pet.currentStamina - staminaSteal);
            _pet.currentStamina += Mathf.Min(_pet.maxStamina, _pet.currentStamina + staminaSteal);
        }
        charges--;
    }
}
