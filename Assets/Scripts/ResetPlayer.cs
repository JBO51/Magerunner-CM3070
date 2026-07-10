//Script for resetting the player on environmental hazard collision, 
//or when player presses R
//Attached to Player Hitbox under Player object in Level 1 + 2 Scene
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ResetPlayer : MonoBehaviour
{
    //Hazard colliders to be used for lvl 2
    [Header("Hazard Reference")]
    [SerializeField] private Collider hazardCollider1;
    [SerializeField] private Collider hazardCollider2;
    [SerializeField] private Collider hazardCollider3;

    void Update()
    {
        //Check for the 'R' key press
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            ResetLevel();
        }
    }

    //Check if player has collision with collider
    private void OnTriggerEnter(Collider obj)
    {
        if (obj == hazardCollider1 || obj == hazardCollider2 || obj == hazardCollider3)
        {
            ResetLevel();
        }
    }

    public void ResetLevel()
    {
        //reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}