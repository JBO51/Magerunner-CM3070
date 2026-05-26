using UnityEngine;
using System.Collections.Generic; //Gets the C# list functionality

//Manage the spells in the players inventory
public class PlayerSpellInventory : MonoBehaviour
{
    [Header("Inventory Settings")]
    [Tooltip("List of currently held spells collected by the player.")]
    //Dynamic list of spells that holds all the spells the player has collected
    //Uses the items defined in the SpellTypes.cs enumeration to make the list
    //So it can only hold items defined in enum
    public List<SpellType> collectedSpells = new List<SpellType>();
    //In plaintext looks like this:
    //[0] = RedPush
    //[1] = Redpush
    //[2] = Bluepull

    //Function to add spell to inventory
    //Public function so other function (SpellPickup.cs) can trigger it
    public void AddSpell(SpellType newSpell)
    {
        //Put new spell from argument in end of inventory list
        collectedSpells.Add(newSpell);
        //Temp debug for 
        Debug.Log($"Collected {newSpell}. Total spells held = {collectedSpells.Count}");

        //More mage book stuff here later after prototype
    }
}