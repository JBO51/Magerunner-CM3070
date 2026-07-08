//UI Manager for the pause menu while in a level
//Attached to PauseUIManager in Player UI object (Level 1 and 2)
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    [Header("Pause Menu Reference")]
    public GameObject pauseMenuPanel;

    [Header("Disable player controls")]
    public MonoBehaviour playerMovement;
    public MonoBehaviour mouseLook;
    public PlayerSpellCasting spellCastingScript;
    public ResetPlayer resetButton;

    private bool isPaused = false;
    
    //Keep pause menu off when you load into level
    void Start()
    {
        pauseMenuPanel.SetActive(false);
    }

    void Update()
    {
        //Check for escape key input to pause game
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (!isPaused)
                PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        pauseMenuPanel.SetActive(true);

        //Stop time in Unity (so all the physics stops)
        Time.timeScale = 0f;

        //Lock the player out of all controls fo rthe pause menu
        playerMovement.enabled = false;
        mouseLook.enabled = false;
        spellCastingScript.enabled = false;
        resetButton.enabled = false;

        //Unlock the mouse so the player can select buttons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);

        //Resume time
        Time.timeScale = 1f;

        //Re-lock mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //make sure to not enable controls while in panning
        if (!LevelIntroPan.IsIntroActive)
        {
            playerMovement.enabled = true;
            mouseLook.enabled = true;
            spellCastingScript.enabled = true;
            resetButton.enabled = true;
        }
    }

    public void ResetLevel()
    {
        //First resume time (buggy if you dont)
        Time.timeScale = 1f;
        //Reload current scene
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
}