using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRaceTile : MonoBehaviour
{
    [SerializeField] GameObject normalTile;
    [SerializeField] GameObject sandTile;
    [SerializeField] GameObject waterTile;
    [SerializeField] GameObject obstacleTile;

    GameObject currentTile;
    char _tileType;
    /*
        0 - Normal
        1 - Sand
        2 - Water
        3 - Obstacle
    */
    public void InitTile(char tileType)
    {
        currentTile = tileType switch
        {
            '1' => Instantiate(sandTile, transform),
            '2' => Instantiate(waterTile, transform),
            '3' => Instantiate(obstacleTile, transform),
            _ => Instantiate(normalTile, transform),
        };
        _tileType = tileType;
    }

    public char EnterTile()
    {
        if (_tileType == '3')
        {
            Destroy(currentTile);
            currentTile = Instantiate(normalTile, transform);
        }
        return _tileType;
    }
}
