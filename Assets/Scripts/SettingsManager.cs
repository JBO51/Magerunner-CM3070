//Script for changing settings (Sensitivity + FOV)
//Seperate script so that it can be used in both Menu + Pause menu
//Attached to SettingsPanel under PauseUIManager, Player UI in Level 1 + 2 scenes
//Attached to SettingsPanel under MenuUI in Main Menu scene
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("UI Slider Targets")]
    public Slider sensitivitySlider;
    public Slider fovSlider;
    public Slider volumeSlider;

    void OnEnable()
    {
        //Uses PlayerPrefs registry to save player settings preferences
        //Starts by finding the registry, and assigning default values if no previous info.
        float savedSens = PlayerPrefs.GetFloat("MouseSensitivity", 3.5f);
        float savedFOV = PlayerPrefs.GetFloat("TargetFOV", 100f);
        float savedVolume = PlayerPrefs.GetFloat("Volume", 0.5f);

        //Change the sliders in settings to match the default/saved settings
        sensitivitySlider.value = savedSens;
        fovSlider.value = savedFOV;
        volumeSlider.value = savedVolume;
    }

    //On Value Changed of the Sensitivity Slider
    public void SetSensitivity(float value)
    {
        //Get slider value and save to registry
        PlayerPrefs.SetFloat("MouseSensitivity", value);
        PlayerPrefs.Save();

        //If they are in a level, MouseLook will be found and changed to change live
        MouseLook activeMouseLook = Object.FindFirstObjectByType<MouseLook>();
        if (activeMouseLook != null) activeMouseLook.mouseSensitivity = value;
    }

    //On Value Changed of the FOV Slider
    //Same concept as sensitivity
    public void SetFieldOfView(float value)
    {
        PlayerPrefs.SetFloat("TargetFOV", value);
        PlayerPrefs.Save();

        MouseLook activeMouseLook = Object.FindFirstObjectByType<MouseLook>();
        if (activeMouseLook != null)
        {
            activeMouseLook.targetFOV = value;
            //this time instantly force cam to change so you can see
            //FOV changing while in pause menu
            Camera cam = activeMouseLook.GetComponent<Camera>();
            if (cam != null) cam.fieldOfView = value;
        }
    }

    //On Value Changed of the Volume Slider
    public void SetVolume(float value)
    {
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();

        //First change it for the Main Menu BGM
        GameObject bgmObject = GameObject.Find("BGMMainMenu");
        if (bgmObject != null)
        {
            AudioSource activeBGM = bgmObject.GetComponent<AudioSource>();
            //same as fov time instantly force cam to change so you can hear
            //change while in pause menu
            if (activeBGM != null) activeBGM.volume = value;
        }

        //Next change it for the Level BGM
        if (BGMController.instance != null)
        {
            BGMController.instance.UpdateVolume(value);
        }

    }

    public void ClearSavedData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        //Check if the scene is the main menu
        MainMenuController mainMenu = Object.FindFirstObjectByType<MainMenuController>();
        //if so, then we update the best times so that its instantly changed
        if (mainMenu != null) mainMenu.UpdateBestTimes();
    }

    //Close settings view in menu
    public void CloseSettings()
    {
        gameObject.SetActive(false);
    }
}