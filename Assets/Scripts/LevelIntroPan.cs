//Script for the initial camera pan from the goal to the player
//Has to start at an intro point, drift backwards, then pan back to player
using UnityEngine;
using System.Collections;

public class LevelIntroPan : MonoBehaviour
{
    //Static boolean for checking if the player has already seen the intro;
    //static so that scene reloading doesnt trigger it again
    private static bool hasPlayedIntro = false;
    //Added active tracker for pause menu bugginess; allows us to
    //activate/deactivate controls better
    private static bool isIntroActive = false;
    public static bool IsIntroActive => isIntroActive;

    [Header("References")]
    //Intro camera starting point
    public Transform introStartPoint;  
    //Player camera starting point
    public Transform playerCameraPosition;
    public LevelTimer timer;

    [Header("Disable player controls")]
    //Disable the movement + mouselook scripts
    public MonoBehaviour playerMovement; 
    public MonoBehaviour mouseLook;
    public PlayerSpellCasting spellCastingScript;

    [Header("Duration")]
    //How long the intro takes
    public float panDuration = 3.5f;
    //How long the initial drift lasts
    public float holdDuration = 2.0f;
    //drift at start so camera not frozen
    public float holdDriftDistance = 2.5f;

    void Start()
    {
        //If the intro already played (i.e. the level got reset), dont play intro
        if (hasPlayedIntro)
        {
            SnapToPlayer();
            return;
        }
        //If not, play intro
        isIntroActive = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(PlayIntroPanRoutine());
    }

    private IEnumerator PlayIntroPanRoutine()
    {
        //Lock the player out of movement and looking around during the pan
        playerMovement.enabled = false;
        mouseLook.enabled = false;
        spellCastingScript.enabled = false;

        //Put camera in starting position
        transform.position = introStartPoint.position;
        transform.rotation = introStartPoint.rotation;

        //Make it global coords
        transform.SetParent(null);

        //Calculate a point directly behind where the camera is facing for drift 
        Vector3 driftedStartPos = introStartPoint.position - (introStartPoint.forward * holdDriftDistance);

        //Drift for 2 seconds
        float holdElapsed = 0f;
        while (holdElapsed < holdDuration)
        {
            holdElapsed += Time.deltaTime;
            float tHold = Mathf.SmoothStep(0f, 1f, holdElapsed / holdDuration);
            //Lerp = linear interpolation, moving between two points along a line
            //This moves from the start pos to the drift pos linearlly
            transform.position = Vector3.Lerp(introStartPoint.position, driftedStartPos, tHold);

            yield return null;
        }

        //Now interpolate pos/rot from the drifted endpoint back to player
        float panElapsed = 0f;
        while (panElapsed < panDuration)
        {
            panElapsed += Time.deltaTime;
            float tPan = panElapsed / panDuration;

            tPan = Mathf.SmoothStep(0f, 1f, tPan);

            //Slerp = spherical linear inerpolation, moves between two points along
            //a sphere for smooth rotation. Normal Lerp for position itself
            transform.position = Vector3.Lerp(driftedStartPos, playerCameraPosition.position, tPan);
            transform.rotation = Quaternion.Slerp(introStartPoint.rotation, playerCameraPosition.rotation, tPan);

            yield return null;
        }

        //Give control to player
        SnapToPlayer();

        //FLip the static boolean so it doesnt play again on reset
        hasPlayedIntro = true;
    }

    private void SnapToPlayer()
    {
        isIntroActive = false;

        //Lock the camera back into the player
        transform.SetParent(playerCameraPosition);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        //Re-enable controls
        playerMovement.enabled = true;
        mouseLook.enabled = true;
        spellCastingScript.enabled = true;

        //Start timer   
        timer.StartTimer();
    }

    //For resetting the intro play when you leave to main menu
    public static void ResetIntro()
    {
        hasPlayedIntro = false;
        isIntroActive = false;
    }
}