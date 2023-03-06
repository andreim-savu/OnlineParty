using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRacePerkFastFinish : PetRacePerk
{
    public override void InitPerk(PetRacePet pet, int level)
    {
        _pet = pet;
        _level = level;
        _pet.onTrackPlaced.AddListener(Action);
    }

    void Action()
    {
        _pet._track.ShortenTrackPercent(_level * 5);
    }
}
