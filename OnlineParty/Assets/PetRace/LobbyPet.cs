using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LobbyPet : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] Transform _modelTransform;
    public void InitPet(string name, int modelNo)
    {
        if (name == null || name == "") { return; }
        _name.text = name;
        Instantiate(PetRacePetModels.Instance.GetModel(modelNo), _modelTransform);
    }
}
