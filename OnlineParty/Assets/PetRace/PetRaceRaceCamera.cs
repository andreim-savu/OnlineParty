using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRaceRaceCamera : MonoBehaviour
{
    [SerializeField] List<PetRacePet> petsToWatch = new List<PetRacePet>();

    [SerializeField] PetRacePet leadingPet;
    public void InitCamera(List<PetRacePet> pets)
    {
        petsToWatch = pets;
        leadingPet = pets[0];
    }

    Vector3 _offset = new Vector3(7.5f, 12, -8);

    private void LateUpdate()
    {
        if (!PetRaceCanvas.Instance._inRace) { return; }

        foreach (PetRacePet pet in petsToWatch)
        {
            if (!pet.isRacing) { continue; }

            if (pet.transform.localPosition.z > leadingPet.transform.localPosition.z * (!leadingPet.isRacing ? 0 : 1)) { leadingPet = pet; }
        }

        transform.position = Vector3.Lerp(transform.position, leadingPet.transform.position + _offset, Time.deltaTime * 10);
    }

    public void StopCamera()
    {
        petsToWatch.Clear();
        leadingPet = null;
        transform.localPosition = Vector3.zero;
    }
}
