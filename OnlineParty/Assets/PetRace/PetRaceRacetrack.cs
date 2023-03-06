using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRaceRacetrack : MonoBehaviour
{
    [SerializeField] PetRaceTrack _track;
    [SerializeField] List<PetRaceTrack> _tracks = new List<PetRaceTrack>();

    /*
        0 - Normal
        1 - Sand
        2 - Water
        3 - Obstacle
    */

    int sandTrackMinLength = 4;
    int waterTrackMinLength = 4;
    int maxObstacles = 7;

    public List<PetRaceTrack> InitRacetrack(List<PetRacePet> pets, int trackLength)
    {
        print("Initiating Racetrack");
        sandTrackMinLength = 4;
        waterTrackMinLength = 4;
        maxObstacles = 7;

        string obstacleString = "0000000";

        int sandTrackRemainingLength = 0;

        int waterTrackRemainingLength = 0;

        for (int i = 7; i < trackLength - 5; i++)
        {
            if (sandTrackRemainingLength > 0)
            {
                obstacleString += 1;
                sandTrackRemainingLength--;
                continue;
            }
            if (waterTrackRemainingLength > 0)
            {
                obstacleString += 2;
                waterTrackRemainingLength--;
                continue;
            }

            int random = Random.Range(0, 101);

            if (random < 80)
            {
                obstacleString += "0";
                continue;
            }

            if (random < 90)
            {
                if (maxObstacles > 0)
                {
                    obstacleString += "3";
                    maxObstacles--;
                    continue;
                }
                obstacleString += "0";
                continue;
            }

            if (random < 95)
            {
                obstacleString += "1";
                sandTrackRemainingLength = sandTrackMinLength - 1;
                continue;
            }

            if (random < 100)
            {
                obstacleString += "2";
                waterTrackRemainingLength = waterTrackMinLength - 1;
                continue;
            }
        }

        obstacleString += "00004";

        for (int i = 0; i < pets.Count; i++)
        {
            PetRaceTrack newTrack = Instantiate(_track, transform);
            newTrack.transform.localPosition = new Vector3(i * 2, 0, 0);
            newTrack.SetTrackString(obstacleString);
            _tracks.Add(newTrack);
        }
        return _tracks;
    }

    public void DestroyTrack()
    {
        foreach (PetRaceTrack track in _tracks)
        {
            Destroy(track.gameObject);
        }
        _tracks.Clear();
    }
}
