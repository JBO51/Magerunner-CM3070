using UnityEngine;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI timerText;

    private float elapsedTime = 0f;
    private bool isTimerRunning = false;

    void Start()
    {
        //Start clock when the level loads
        StartTimer();
    }

    void Update()
    {
        if (isTimerRunning)
        {
            elapsedTime += Time.deltaTime;
            //calculate minutes seconds millisecondss
            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            int hundredths = Mathf.FloorToInt((elapsedTime * 100f) % 100f);
            //Format to 00:00.00
            timerText.text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, hundredths);
        }
    }

    public void StartTimer()
    {
        elapsedTime = 0f;
        isTimerRunning = true;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    //grab the final time
    public float GetFinalTime()
    {
        return elapsedTime;
    }
}