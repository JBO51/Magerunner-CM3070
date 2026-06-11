//Script for managing player spell inventory, picking up spells and cycling through spells

using UnityEngine;
using UnityEngine.InputSystem;
//Gets the C# list functionality
using System.Collections.Generic; 

//Manage the spells in the players inventory
public class PlayerSpellInventory : MonoBehaviour
{
    [Header("Inventory Settings")]
    //Dynamic list of spells that holds all the spells the player has collected
    //Uses the items defined in the SpellTypes.cs enumeration to make the list
    //So it can only hold items defined in enum
    public List<SpellType> collectedSpells = new List<SpellType>();
    //In plaintext looks like this:
    //[0] = SpellType.RedPush
    //[1] = SpellType.Redpush
    //[2] = SpellType.Bluepull

    //Variable for current active element (-1 means inventory is empty)
    public int currentSpellIndex = -1;

    //public read-only property so other scripts can look at what is equipped
    public SpellType EquippedSpell
    {
        get
        {
            //Check if we have spells
            if (collectedSpells.Count > 0)
            {
                return collectedSpells[currentSpellIndex];
            }
            //Otherwise equipped spell is none
            return SpellType.None;
        }
    }

    //C# Action (delegate event) that broadcasts to the UI whenever an item is added or cycled
    public System.Action OnSpellChanged;

    //Update for key press cycling spells
    void Update()
    {
        //If the player doesn't have any spells yet, or only 1, ignore q and e presses
        if (collectedSpells.Count <= 1) return;

        //Poll for Q/E inputs to cycle spells
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            CycleSpell(-1); //Cycle backward
        }
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            CycleSpell(1);  //Cycle forward
        }
    }

    //Functionlity of cycling spells
    //Direction argument for which direction its going
    private void CycleSpell(int direction)
    {
        //check what the next spell is
        int nextIndex = currentSpellIndex + direction;

        //if its out of bounds, then return (just in case)
        if (nextIndex < 0 || nextIndex >= collectedSpells.Count) return;

        //update current spell based on direction
        currentSpellIndex = nextIndex;

        //Notify UI to refresh visuals
        OnSpellChanged?.Invoke();
    }

    //Function to add spell to inventory
    //Public function so other function (SpellPickup.cs) can trigger it
    public void AddSpell(SpellType newSpell)
    {
        //Put new spell from argument in end of inventory list
        collectedSpells.Add(newSpell);

        //Set the current active index to last in list so the new spell is equipped
        currentSpellIndex = collectedSpells.Count - 1;

        //Notify UI to refresh visuals
        OnSpellChanged?.Invoke();
    }

    //Remove the currently equipped spell page from the list for spellcasting
    public void ConsumeEquippedSpell()
    {
        //Remove the spell from list
        collectedSpells.RemoveAt(currentSpellIndex);

        //reconnect index pointer now that size changed
        if (collectedSpells.Count == 0)
        {
            currentSpellIndex = -1; 
        }
        else
        {
            //Step back by 1 to stay in boundary
            if (currentSpellIndex >= collectedSpells.Count)
            {
                currentSpellIndex = collectedSpells.Count - 1;
            }
        }

        //Notify UI to refresh visuals
        OnSpellChanged?.Invoke();
    }
}