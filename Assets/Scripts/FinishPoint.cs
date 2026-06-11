//Script for the finish point functionality in levels
using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    [Header("References")]
    public VictoryMenuController victoryScreen;
    public LevelTimer timer;

    private void OnTriggerEnter(Collider obj)
    {
        //Check if player passed finish line
        if (obj.GetComponent<PlayerMovement>() != null)
        {
            //Stop timer
            timer.StopTimer();
            //Pass time to victory screen for the results screen
            victoryScreen.DisplayVictoryScreen(timer.GetFinalTime());
        }
    }
}