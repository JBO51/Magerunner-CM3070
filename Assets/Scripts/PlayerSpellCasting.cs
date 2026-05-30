using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerSpellCasting : MonoBehaviour
{
    [Header("Main Camera Reference")]
    public Camera playerCamera;

    private PlayerSpellInventory inventory;

    [Header("Red Push Settings")]
    public float castRange = 25f;
    public float pushForce = 20f;
    public float pushRadius = 1.5f;

    void Start()
    {
        //hook to inventory script 
        inventory = GetComponent<PlayerSpellInventory>();
    }

    //Check for mouse input in update
    void Update()
    {
        //Poll for a left click input to cast current spell 
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TryCastSpell();
        }
    }

    //Validation before casting equipped spell
    private void TryCastSpell()
    {
        //Fetch what current spell is 
        SpellType activeSpell = inventory.EquippedSpell;

        //If nothing is equipped, return
        if (activeSpell == SpellType.None) return;

        //Check which spell type current spell is
        if (activeSpell == SpellType.RedPush)
        {
            RedPush();
        }

        //BluePull and GreenStasis
    }

    private void RedPush()
    {
        ////NEED TO FIX COLLISION DETECTION THROUGH WALLS

        //create a ray extending forward from the cdenter of the viewport
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        //array to collect all hits on objects along the ray path
        RaycastHit[] hits = Physics.SphereCastAll(ray, pushRadius, castRange);

        //Loop through every object intersected with
        foreach (RaycastHit hit in hits)
        {
            //Prevent the spell from also hitting the player 
            if (hit.collider.transform.root == transform.root) continue;

            //Check if object has a rigidbody component
            if (hit.collider.TryGetComponent<Rigidbody>(out Rigidbody targetRb))
            {
                //Add push based on ray direction, and force variables
                Vector3 pushDirection = ray.direction;
                targetRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
            }
        }

        //4. Remove spell from inventory
        inventory.ConsumeEquippedSpell();
    }

    private void BluePull()
    {

    }
    private void GreenStasis()
    {
        
    }
}