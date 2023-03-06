using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRacePerkLoudRunner : PetRacePerk
{
    float movementSpeedModifier;
    float duration;
    int charges;
    public override void InitPerk(PetRacePet pet, int level)
    {
        _pet = pet;
        _level = level;
        switch (_level)
        {
            case 1:
                movementSpeedModifier = 0.5f;
                duration = 2;
                charges = 1;
                break;
            case 2:
                movementSpeedModifier = 0.5f;
                duration = 2;
                charges = 2;
                break;
            case 3:
                movementSpeedModifier = 0.5f;
                duration = 4;
                charges = 2;
                break;
            case 4:
                movementSpeedModifier = 0.5f;
                duration = 4;
                charges = 3;
                break;
            case 5:
                movementSpeedModifier = 0.25f;
                duration = 4;
                charges = 3;
                break;
        }

        _pet.onStartRunning.AddListener(Active);
    }

    void Active()
    {
        if (charges == 0) { return; }
        StartCoroutine(ActiveRoutine());
        charges--;
    }

    IEnumerator ActiveRoutine()
    {
        foreach (PetRacePet pet in PetRaceCanvas.Instance._petObjects)
        {
            if (pet == _pet) { continue; }
            pet.loudRunnerModifier = movementSpeedModifier;
        }
        yield return new WaitForSeconds(duration);
        foreach (PetRacePet pet in PetRaceCanvas.Instance._petObjects)
        {
            if (pet == _pet) { continue; }
            pet.loudRunnerModifier = 1;
        }
    }
}
