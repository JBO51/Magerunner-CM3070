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

    void OnEnable()
    {
        //Uses PlayerPrefs registry to save player settings preferences
        //Starts by finding the registry, and assigning default values if no previous info.
        float savedSens = PlayerPrefs.GetFloat("MouseSensitivity", 3.5f);
        float savedFOV = PlayerPrefs.GetFloat("TargetFOV", 100f);

        //Change the sliders in settings to match the default/saved settings
        sensitivitySlider.value = savedSens;
        fovSlider.value = savedFOV;
    }

    //On Value Changed of the Sensitivity Slider
    public void SetSensitivity(float value)
    {
        //Get slider value and save to registry
        PlayerPrefs.SetFloat("MouseSensitivity", value);
        PlayerPrefs.Save();

        //If they are in a level, MouseLook will be found and changed to change live
        MouseLook activeMouseLook = Object.FindFirstObjectByType<MouseLook>();
        if (activeMouseLook != null)
        {
            activeMouseLook.mouseSensitivity = value;
        }
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
    
    public void ClearSavedData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    //Close settings view in menu
    public void CloseSettings()
    {
        gameObject.SetActive(false);
    }
}