//Script for managing player spell inventory, picking up spells and cycling through spells

using UnityEngine;
using UnityEngine.InputSystem; //for handling spell cycle input
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
    //[0] = SpellType.RedPush
    //[1] = SpellType.Redpush
    //[2] = SpellType.Bluepull

    //Variable for current active element (-1 means inventory is empty)
    private int currentSpellIndex = -1;

    //public read-only property so other scripts can look at what is equipped
    public SpellType EquippedSpell
    {
        get
        {
            //Check if the index is within bounds of the list
            if (currentSpellIndex >= 0 && currentSpellIndex < collectedSpells.Count)
            {
                return collectedSpells[currentSpellIndex];
            }
            return SpellType.None;
        }
    }

    //C# Action (delegate event) that broadcasts to the UI whenever an item is added or cycled
    public System.Action OnSpellChanged;

    //Update for key press cycling spells
    void Update()
    {
        //If the player doesn't have any spells yet, ignore q and e presses
        if (collectedSpells.Count == 0) return;

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

    //Function to add spell to inventory
    //Public function so other function (SpellPickup.cs) can trigger it
    public void AddSpell(SpellType newSpell)
    {
        //Put new spell from argument in end of inventory list
        collectedSpells.Add(newSpell);
        //Temp debug for 
        Debug.Log($"Collected {newSpell}. Total spells held = {collectedSpells.Count}");

        //Set the current active index to last in list so the new spell is equipped
        currentSpellIndex = collectedSpells.Count - 1;

        //Notify UI to display updated spell (UpdateUI() method)
        OnSpellChanged?.Invoke();
    }

    //Private function for functioanlity of cycling spells
    //Direction argument for which direction its going
    private void CycleSpell(int direction)
    {
        //Just return if you only hold 1 or 0 spells
        if (collectedSpells.Count <= 1) return;

        //check what the next spell is
        int nextIndex = currentSpellIndex + direction;

        //if its out of bounds, then return
        if (nextIndex < 0 || nextIndex >= collectedSpells.Count) return;

        //update current spell based on direction
        currentSpellIndex = nextIndex;

        //Notify UI to refresh visuals
        OnSpellChanged?.Invoke();
    }
}