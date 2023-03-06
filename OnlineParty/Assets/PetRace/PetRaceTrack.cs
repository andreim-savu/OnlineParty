using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRaceTrack : MonoBehaviour
{
    [SerializeField] PetRaceTile tile;
    public int trackLength;
    string _trackString;

    List<PetRaceTile> tiles = new List<PetRaceTile>();

    public void SetTrackString(string trackString)
    {
        _trackString = trackString;
        trackLength = trackString.Length;
    }

    public void InitTrack()
    {
        for (int i = 0; i < trackLength; i++)
        {
            PetRaceTile newTile = (_trackString[i]) switch
            {
                '1' => Instantiate(tile, transform),
                '2' => Instantiate(tile, transform),
                '3' => Instantiate(tile, transform),
                _ => Instantiate(tile, transform),
            };
            newTile.transform.localPosition = new Vector3(0, 0, i);
            newTile.InitTile(_trackString[i]);
            tiles.Add(newTile);
        }
    }

    public char GetTile(string index)
    {
        return tiles[int.Parse(index)].EnterTile();
    }

    public void ShortenTrackPercent(int percent)
    {
        int tilesToTakeOut = percent;
        _trackString = _trackString.Remove(10, tilesToTakeOut);
        trackLength -= tilesToTakeOut;
    }
}
