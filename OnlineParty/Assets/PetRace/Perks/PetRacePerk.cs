using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PetRacePerk : MonoBehaviour
{
    protected PetRacePet _pet;
    protected int _level;

    public abstract void InitPerk(PetRacePet pet, int level);
}
