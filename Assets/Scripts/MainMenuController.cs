//UI controller for the main menu
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    void Start()
    {
        //Re-enable and unlock the hardware mouse cursor 
        //so players can interact with the UI after quitting out
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //Call load level 1 for start button too
    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void LoadLevel2()
    {
        //SceneManager.LoadScene("Level2");
    }

    //Close the game
    public void QuitGame()
    {
        Application.Quit();
    }
}