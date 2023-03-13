using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRaceRaceCamera : MonoBehaviour
{
    Camera _cam;
    [SerializeField] List<PetRacePet> petsToWatch = new List<PetRacePet>();

    [SerializeField] PetRacePet leadingPet;
    [SerializeField] PetRacePet lastPet;
    public void InitCamera(List<PetRacePet> pets)
    {
        _cam = GetComponent<Camera>();
        petsToWatch = pets;
        leadingPet = pets[0];
        lastPet = pets[pets.Count - 1];
    }

    Vector3 _offset = new Vector3(30.5f, 43.75f, -30.5f);
    float _baseCameraSize = 10.5f;

    private void LateUpdate()
    {
        if (!PetRaceCanvas.Instance._inRace) { return; }

        foreach (PetRacePet pet in petsToWatch)
        {
            if (!pet.isRacing) { continue; }

            if (pet.transform.localPosition.z > leadingPet.transform.localPosition.z * (!leadingPet.isRacing ? 0 : 1)) { leadingPet = pet; }
            if (pet.transform.localPosition.z < lastPet.transform.localPosition.z) { lastPet = pet; }
        }

        float petDistance = leadingPet.transform.localPosition.z - lastPet.transform.localPosition.z;
        if (petDistance > 15.0f) { _cam.orthographicSize = Mathf.Lerp(_cam.orthographicSize, _baseCameraSize + (petDistance - 15) / 2.0f, Time.deltaTime * 10);}
        else { _cam.orthographicSize = _cam.orthographicSize = Mathf.Lerp(_cam.orthographicSize, _baseCameraSize, Time.deltaTime * 10); }

        transform.position = Vector3.Lerp(transform.position, leadingPet.transform.position + _offset, Time.deltaTime * 10);
    }

    public void StopCamera()
    {
        petsToWatch.Clear();
        leadingPet = null;
        transform.localPosition = Vector3.zero;
    }
}
