using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PetRaceLeaderboardPlayer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _place;
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] TextMeshProUGUI _score;

    public void SetUp(int place, IPetRacePlayer player)
    {
        _place.text = place.ToString();
        _name.text = player.name;
        _score.text = player.score.ToString();

        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (place - 1) * -40);
    }
}
