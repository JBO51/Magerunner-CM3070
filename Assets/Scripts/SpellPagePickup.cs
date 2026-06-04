using UnityEngine;

//Manage picking up a spell and the appearance of a spell in game
public class SpellPagePickup : MonoBehaviour
{
    [Header("Spell Type")]
    //Using spell enumeration from SpellTypes, dropdown menu for which spell
    public SpellType spellToGrant;

    //Visual effects for the floating animation
    [Header("Visual Effects")]
    public float rotationSpeed = 50f;
    public float floatAmplitude = 0.2f;
    public float floatFrequency = 2f;

    private Vector3 startPosition;

    //Initial x y and z coords of the item for the floating animation 
    void Start()
    {
        startPosition = transform.position;
    }

    //Animation of the spell updated every frame
    void Update()
    {
        //Spin item around y axis based on speed and time
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        //Sin wave based floating cycle so it gradually floats up and down
        Vector3 tempPos = startPosition;
        tempPos.y += Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = tempPos;
    }

    //Ontrigger collision function for when player actually collides with spell
    private void OnTriggerEnter(Collider obj)
    {
        //Dont need to check what the collider is here since this is a monobehaviour script
        //Verify if the player is touching the object
        //can do it by checking if object touching the pickup has inventory system
        if (obj.TryGetComponent<PlayerSpellInventory>(out PlayerSpellInventory playerInventory))
        {
            //Transfer the spell to the player with PlayerSpellInventory.cs function
            playerInventory.AddSpell(spellToGrant);

            //Destroy the page pickup object from the scene
            Destroy(gameObject);
        }
    }
}