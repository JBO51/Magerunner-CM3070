//Adapted from guide: https://www.youtube.com/watch?v=f473C43s8nE
using UnityEngine;
using UnityEngine.InputSystem; //Modern input hardware

public class MouseLook : MonoBehaviour
{
    //Label in the unity inspector to easily change sensitivity of mouse in camera movement
    //Slider for sensitivity is between 0.5f and 10f, set at 2f default
    [Header("Look Configurations")]
    [Range(0.5f, 10f)] public float mouseSensitivity = 3.5f;
    [Range(60f, 110f)] public float targetFOV = 100f;

    //Attach player to camera since its not a child
    [Header("Tracking Target")]
    public Transform playerBody; 
    public Vector3 headOffset = new Vector3(0f, 0.8f, 0f);

    //Current look angle (pitch)
    private float xRotation = 0f;
    private float yRotation = 0f;

    //Internal multiplier to scale down sensitivity value
    private const float InputScaleFactor = 0.01f;

    void Start()
    {
        //Cursor focused onto screen so user cannot click outside of the window
        Cursor.lockState = CursorLockMode.Locked;
        //hide cursor
        Cursor.visible = false;

        //Initialize rotation variables to match current scene setup
        xRotation = transform.localEulerAngles.x;
        yRotation = playerBody.eulerAngles.y;

        //Apply FOV
        Camera cam = GetComponent<Camera>();
        cam.fieldOfView = targetFOV;
    }

    //process mouse rotation every rendered frame
    void Update()
    {
        //Poll mouse delta so we know the rotation, and multiply by mouse sensitivity
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        float mouseX = mouseDelta.x * mouseSensitivity * InputScaleFactor;
        float mouseY = mouseDelta.y * mouseSensitivity * InputScaleFactor;

        //Rotating around X-axis = tilting camera up and down
        //Subtract xRotation by mouseY because moving the cam up returns a positive num
        //and looking up needs a negative x rotation (and vice versa)
        xRotation -= mouseY;
        //Clamp rotation between 90 and -90 degrees so player cant flip camera
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        yRotation += mouseX;

        //Apply clamped rotation, convert degrees into unity math with Quaternion 
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        transform.position = playerBody.position + headOffset;
    }
}