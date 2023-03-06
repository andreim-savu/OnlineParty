using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LobbyPet : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _name;
    public void InitPet(string name)
    {
        _name.text = name;
    }
}
