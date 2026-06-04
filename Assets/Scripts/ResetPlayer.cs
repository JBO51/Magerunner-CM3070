using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ResetPlayer : MonoBehaviour
{
    [Header("Lava Reference")]
    [SerializeField] private Collider lavaCollider;

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
        if (obj == lavaCollider)
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