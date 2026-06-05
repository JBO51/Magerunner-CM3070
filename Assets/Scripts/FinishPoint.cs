//Script for the finish point functionality in levels
using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider obj)
    {
        //Check if player passed finish line
        if (obj.CompareTag("Player") || obj.GetComponent<PlayerMovement>() != null)
        {
            //Stop timer
            LevelTimer timer = Object.FindFirstObjectByType<LevelTimer>();
            if (timer != null)
            {
                timer.StopTimer();


                //NEED TO ADD: victory screen + load next level 
            }
        }
    }
}