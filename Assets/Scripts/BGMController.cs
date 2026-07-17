//Script to keep BGM for levels playing when resetting and
//going to next level, then removing itself on the main menu
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMController : MonoBehaviour
{
    //Make public so that we can apply volume adjustments
    public static BGMController instance;
    private AudioSource audioSource;

    void Start()
    {
        //Make sure that BGM isnt duplicated in scenes
        //when resetting
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        //DonDestroyOnLoad keeps this object when 
        //loading new scenes
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        //get audio that is attached to this object
        //fetch the stored playerprefs for the volume
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat("Volume", 0.5f);
    }

    //Listener for scene loaded
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    //Unsubscribe or null reference exceptions happen
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //Load when a scene loads to make sure we arent in main menu
    //Since main menu has its own theme, this bgm needs to be destroyed
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            Destroy(this.gameObject);
        }
    }

    //For updating the volume live
    public void UpdateVolume(float newVolume)
    {
        if (audioSource != null)
        {
            audioSource.volume = newVolume;
        }
    }
}
