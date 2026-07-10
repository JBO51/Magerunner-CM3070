//Script for managing the victory screen when finishing a level
//as well as the best times saved in playerPrefs
//Attached to Player UI -> VictoryPanel in Level 1 + 2 Scene
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class VictoryMenuController : MonoBehaviour
{
    [Header("Level")]
    public int levelNumber = 1;

    [Header("UI References")]
    public TextMeshProUGUI clearTimeText;
    public TextMeshProUGUI bestTimeText;
    public TextMeshProUGUI medalText;

    [Header("Hide Next Level")]
    public GameObject nextLevelButton;

    [Header("Medal Thresholds")]
    public float diamondThreshold = 25f;
    public float goldThreshold = 40f;
    public float silverThreshold = 55f;

    [Header("Disable player controls")]
    public MonoBehaviour playerMovement;
    public MonoBehaviour mouseLook;
    public PlayerSpellCasting spellCastingScript;
    public ResetPlayer resetButton;
    public PauseMenuController pauseMenu;

    public void DisplayVictoryScreen(float finalTime)
    {
        //Activate the victory panel 
        gameObject.SetActive(true);

        //Stop time in Unity (so all the physics stops)
        Time.timeScale = 0f;

        //Lock the player out of all controls fo rthe pause menu
        playerMovement.enabled = false;
        mouseLook.enabled = false;
        spellCastingScript.enabled = false;
        resetButton.enabled = false;
        pauseMenu.enabled = false;

        //Unlock the mouse so the player can select buttons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //Format the clear time (to 00:00:00)
        clearTimeText.text = "Your Time: " + FormatTime(finalTime);

        //Use PlayerPrefs to check and store the best time for the current level
        //Can use a key for each level
        string levelBestKey = "BestTimeLevel" + levelNumber;
        //(start best time out at 9999 so the new best is guaranteed for first run)
        float bestTime = PlayerPrefs.GetFloat(levelBestKey, 9999f);

        if (finalTime < bestTime)
        {
            bestTime = finalTime;
            PlayerPrefs.SetFloat(levelBestKey, finalTime);
            PlayerPrefs.Save();
            bestTimeText.text = "NEW PERSONAL BEST!";
        }
        else
        {
            bestTimeText.text = "Best Time: " + FormatTime(bestTime);
        }

        //Find the medal earned based on the final time and the boundaries changed in the header
        MedalBoundaries(finalTime);

        //Hide next level button if its level 2 since there is no next level
        if (levelNumber == 2)
        {
            nextLevelButton.SetActive(false);
        }
    }

    private void MedalBoundaries(float clearTime)
    {
        if (clearTime <= diamondThreshold)
        {
            medalText.text = "DIAMOND MEDAL";
        }
        else if (clearTime <= goldThreshold)
        {
            medalText.text = "GOLD MEDAL";
        }
        else if (clearTime <= silverThreshold)
        {
            medalText.text = "SILVER MEDAL";
        }
        else
        {
            medalText.text = "BRONZE MEDAL";
        }
    }

    //Button to load next level
    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        //Have to reset the intro pan for the next level
        LevelIntroPan.ResetIntro();
        SceneManager.LoadScene("Level2");
    }

    public void ResetLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        //Resume time (again, buggy if you dont)
        Time.timeScale = 1f;
        //Reset the level intro so reloading the level shows the goal again
        //(this is only when you back out to the menu)
        LevelIntroPan.ResetIntro();
        //Load main menu
        SceneManager.LoadScene("MainMenu");
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int hundredths = Mathf.FloorToInt((time * 100f) % 100f);
        return string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, hundredths);
    }
}