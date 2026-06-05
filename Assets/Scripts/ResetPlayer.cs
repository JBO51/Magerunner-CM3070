//Script for resetting the player on environmental hazard collision, 
//or when player presses T
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ResetPlayer : MonoBehaviour
{
    [Header("Hazard Reference")]
    [SerializeField] private Collider hazardCollider1;
    [SerializeField] private Collider hazardCollider2;
    [SerializeField] private Collider hazardCollider3;

    void Update()
    {
        //Check for the 'T' key press
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            ResetLevel();
        }
    }

    //Check if player has collision with Lava
    private void OnTriggerEnter(Collider obj)
    {
        if (obj == (hazardCollider1 || hazardCollider2 || hazardCollider3))
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