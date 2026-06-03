using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ResetPlayer : MonoBehaviour
{
    void Update()
    {
        //Check for the 'T' key press
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            ResetLevel();
        }
    }

    //Check if player has collision with Lava
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Lava")
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