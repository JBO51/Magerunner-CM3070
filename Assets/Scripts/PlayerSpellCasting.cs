using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerSpellCasting : MonoBehaviour
{
    [Header("Main Camera Reference")]
    public Camera playerCamera;

    private PlayerSpellInventory inventory;

    [Header("Red Push Settings")]
    public float pushRange = 25f;
    public float pushForce = 20f;
    public float pushRadius = 1.5f;

    [Header("Blue Pull Settings")]
    public float pullRange = 25f;
    public float pullForce = 20f;
    public float pullRadius = 1.5f;

    [Header("Green Stasis Settings")]
    public float stasisRange = 25f;
    public float stasisRadius = 1.5f;

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
        else if (activeSpell == SpellType.BluePull)
        {
            BluePull();
        }
        else if (activeSpell == SpellType.GreenStasis)
        {
            GreenStasis();
        }
    }

    private void RedPush()
    {
        ////NEED TO FIX COLLISION DETECTION THROUGH WALLS

        //create a ray extending forward from the cdenter of the viewport
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        //array to collect all hits on objects along the ray path
        RaycastHit[] hits = Physics.SphereCastAll(ray, pushRadius, pushRange);

        //Loop through every object intersected with
        foreach (RaycastHit hit in hits)
        {
            //Prevent the spell from also hitting the player 
            if (hit.collider.transform.root == transform.root) continue;

            //Check if object has a rigidbody component
            if (hit.collider.TryGetComponent<Rigidbody>(out Rigidbody targetRb))
            {
                //check if isKinematic (for objects that can only be moved by spells)
                if (targetRb.isKinematic)
                {
                    targetRb.isKinematic = false;
                }

                //Add push based on ray direction, and force variables
                Vector3 pushDirection = ray.direction;
                Vector3 forceVector = pushDirection * pushForce;

                //If the object is in stasis, add to cumulative force
                if (hit.collider.TryGetComponent<StasisHandler>(out StasisHandler stasis))
                {
                    stasis.AddStoredForce(forceVector);
                }
                else
                {
                    targetRb.AddForce(forceVector, ForceMode.Impulse);
                }
            }
        }

        //Remove spell from inventory
        inventory.ConsumeEquippedSpell();
    }

    private void BluePull()
    {
        ////NEED TO FIX COLLISION DETECTION THROUGH WALLS

        //create a ray extending forward from the cdenter of the viewport
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        //array to collect all hits on objects along the ray path
        RaycastHit[] hits = Physics.SphereCastAll(ray, pullRadius, pullRange);

        //Loop through every object intersected with
        foreach (RaycastHit hit in hits)
        {
            //Prevent the spell from also hitting the player 
            if (hit.collider.transform.root == transform.root) continue;

            //Check if object has a rigidbody component
            if (hit.collider.TryGetComponent<Rigidbody>(out Rigidbody targetRb))
            {
                //check if isKinematic (for objects that can only be moved by spells)
                if (targetRb.isKinematic)
                {
                    targetRb.isKinematic = false;
                }

                //Multiply ray direction negative to point vector towrds the view line
                Vector3 pullDirection = -ray.direction;
                Vector3 forceVector = pullDirection * pullForce;


                if (hit.collider.TryGetComponent<StasisHandler>(out StasisHandler stasis))
                {
                    stasis.AddStoredForce(forceVector);
                }
                else
                {
                    targetRb.AddForce(forceVector, ForceMode.Impulse);
                }
            }
        }

        //Remove spell from inventory
        inventory.ConsumeEquippedSpell();
    }
    private void GreenStasis()
    {
        //Just 1 object so we can do raycast hit instead of speherecast
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, stasisRange))
        {
            //ignore player
            if (hit.collider.transform.root == transform.root) return;

            if (hit.collider.TryGetComponent<Rigidbody>(out Rigidbody targetRb))
            {
                //Attach the stasis logic script on hit for complex stasis stuff
                if (!hit.collider.gameObject.TryGetComponent<StasisHandler>(out StasisHandler existingHandler))
                {
                    hit.collider.gameObject.AddComponent<StasisHandler>();
                }
            }
        }

        //Remove spell from inventory
        inventory.ConsumeEquippedSpell();
    }
}