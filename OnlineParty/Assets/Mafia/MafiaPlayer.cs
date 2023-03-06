using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MafiaPlayer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI name;
    public string id;


    public void InitPlayer(string playerName, string playerId)
    {
        name.text = playerName;
        id = playerId;
    }

    public void setTextDead()
    {
        name.color = Color.red;
    }
}
