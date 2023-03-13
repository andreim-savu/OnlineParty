using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRacePerkSlowStart : PetRacePerk
{
    float baseSpeedRef;
    float movementSpeedModifier;
    // Start is called before the first frame update
    public override void InitPerk(PetRacePet pet, int level)
    {
        _pet = pet;
        _level = level;
        switch (_level)
        {
            case 1:
                movementSpeedModifier = 1.1f;
                break;
            case 2:
                movementSpeedModifier = 1.2f;
                break;
            case 3:
                movementSpeedModifier = 1.3f;
                break;
            case 4:
                movementSpeedModifier = 1.4f;
                break;
            case 5:
                movementSpeedModifier = 1.5f;
                break;
        }
        _pet.onStatsCalculated.AddListener(Active);
    }

    // Update is called once per frame
    void Active()
    {
        StartCoroutine(ActiveRoutine());
    }

    IEnumerator ActiveRoutine()
    {
        baseSpeedRef = _pet.movementSpeed;
        _pet.movementSpeed = _pet.movementSpeed / 2;
        yield return new WaitForSeconds(8);
        _pet.movementSpeed = baseSpeedRef * movementSpeedModifier;
    }
}
