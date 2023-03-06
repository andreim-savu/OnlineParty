using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;

    [SerializeField] GameObject gameSelectCanvas;

    [SerializeField] GameCanvas mafiaCanvas;
    [SerializeField] GameObject mafiaEnvironment;

    [SerializeField] GameCanvas petRaceCanvas;
    [SerializeField] GameObject petRaceEnvironment;

    private void Awake()
    {
        if (!Instance) { Instance = this; }
        else { Destroy(this); }
    }

    async public void GoToMafia()
    {
        gameSelectCanvas.SetActive(false);
        mafiaCanvas.gameObject.SetActive(true);

        await DatabaseManager.Instance.CreateRoom("Mafia");

        mafiaEnvironment.SetActive(true);
        _ = mafiaCanvas.InitCanvas();
    }

    async public void GoToPetRace()
    {
        gameSelectCanvas.SetActive(false);
        petRaceCanvas.gameObject.SetActive(true);

        await DatabaseManager.Instance.CreateRoom("PetRace");

        petRaceEnvironment.SetActive(true);
        _ = petRaceCanvas.InitCanvas();
    }
}
