using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRacePetModels : MonoBehaviour
{
    public static PetRacePetModels Instance;
    [SerializeField] List<GameObject> _petModels = new List<GameObject>();

    private void Awake()
    {
        if (!Instance) { Instance = this; }
        else { Destroy(this); }
    }

    public GameObject GetModel(int index)
    {
        return _petModels[index];
    }
}
