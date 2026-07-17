//UI controller for the main menu
//Attached to MenuManager in MainMenu scene
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("Personal Best Text UI")]
    public TextMeshProUGUI level1BestTimeText;
    public TextMeshProUGUI level2BestTimeText;

    [Header("BGM Reference")]
    public AudioSource mainMenuBGM;

    void Start()
    {
        //Re-enable and unlock the hardware mouse cursor 
        //so players can interact with the UI after quitting out
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //reupdate the best times upon loading the main menu
        UpdateBestTimes();

        //reupdate the BGM volume for the case when volume was
        //changed in a level
        mainMenuBGM.volume = PlayerPrefs.GetFloat("Volume", 0.5f);
    }

    //Call load level 1 for start button too
    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level2");
    }

    //Close the game
    public void QuitGame()
    {
        Application.Quit();
    }

    //Update the personal best time in the select level menu
    //Public so that SettingsManager can call when clearing data
    public void UpdateBestTimes()
    {
        //Check PlayerPrefs for Level 1 best time
        float bestTimeLvl1 = PlayerPrefs.GetFloat("BestTimeLevel1", 9999f);
        //check if no best time has been set
        if (bestTimeLvl1 < 9999f)
        {
            level1BestTimeText.text = "Personal Best: " + FormatTime(bestTimeLvl1);
        }
        else
        {
            level1BestTimeText.text = "Personal Best: --:--.--";
        }

        //Check PlayerPrefs for Level 2 best time
        float bestTimeLvl2 = PlayerPrefs.GetFloat("BestTimeLevel2", 9999f);
        //check if no best time has been set
        if (bestTimeLvl2 < 9999f)
        {
            level2BestTimeText.text = "Personal Best: " + FormatTime(bestTimeLvl2);
        }
        else
        {
            level2BestTimeText.text = "Personal Best: --:--.--";
        }
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int hundredths = Mathf.FloorToInt((time * 100f) % 100f);
        return string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, hundredths);
    }
}