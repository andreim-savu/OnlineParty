using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRacePerkRunBurst : PetRacePerk
{
    float extraSpeed;
    public override void InitPerk(PetRacePet pet, int level)
    {
        _pet = pet;
        _level = level;
        extraSpeed = _level * 0.5f;
        _pet.onStartRunning.AddListener(Active);
    }

    void Active()
    {
        StartCoroutine(ActiveRoutine());
    }

    IEnumerator ActiveRoutine()
    {
        _pet.extraRunFlatSpeed += extraSpeed;
        yield return new WaitForSeconds(1);
        _pet.extraRunFlatSpeed -= extraSpeed;
    }
}
