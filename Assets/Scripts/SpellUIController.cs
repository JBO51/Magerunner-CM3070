//Listener script for hooking into the playerinventory System.Action broadcast.
//For now just changes UI image box color to match spell type
//Later will be a proper mage book asset

using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class SpellUIController : MonoBehaviour
{
    //UI image component + text
    [Header("UI Graphic Target")]
    public Image cardDisplayImage;
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI clickHintText;
    public TextMeshProUGUI cyclePromptTextQ;
    public TextMeshProUGUI cyclePromptTextE;


    //Colors for diff spells
    [Header("Card Color Palettes")]
    public Color emptySlotColor = new Color(0.15f, 0.15f, 0.15f, 0.6f);
    public Color redPushColor;
    public Color bluePullColor;
    public Color greenStasisColor;

    //Create private version of playerinventory
    private PlayerSpellInventory playerInventory;
    private string clickHint = "Left Click to Cast";

    void Start()
    {
        //Find the player inventory component in scene execution and bind to
        //the private playerInventory
        playerInventory = Object.FindFirstObjectByType<PlayerSpellInventory>();
        playerInventory.OnSpellChanged += UpdateUI;

        //Execute initially to style the card as empty startup
        UpdateUI();
    }

    void OnDestroy()
    {
        //cleanup event links when scenes change to prevent errors
        playerInventory.OnSpellChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        //If there is a spell, change it to the equipped spell
        SpellType activeSpell = playerInventory.EquippedSpell;

        //Switch properties of card UI based on active Enum flag
        switch (activeSpell)
        {
            case SpellType.RedPush:
                //Change the color of the card
                cardDisplayImage.color = redPushColor;
                //Change the text on the card
                cardNameText.text = "PUSH";
                //Add the click to cast hint at the top
                clickHintText.text = clickHint;
                break;
            case SpellType.BluePull:
                cardDisplayImage.color = bluePullColor;
                cardNameText.text = "PULL";
                clickHintText.text = clickHint;
                break;
            case SpellType.GreenStasis:
                cardDisplayImage.color = greenStasisColor;
                cardNameText.text = "FREEZE";
                clickHintText.text = clickHint;
                break;
            case SpellType.None:
            default:
                cardDisplayImage.color = emptySlotColor;
                cardNameText.text = "";
                clickHintText.text = "";
                break;
        }
        //Bounding code for cycle button hints
        //First check if player has more than 1 spell (cant cycle otherwise)
        if (playerInventory.collectedSpells.Count > 1)
        {
            //Initialise booleans can go left/right to determine q/e hints
            //Fetched from the public int in PlayerSpellInventory
            bool canGoLeft = playerInventory.currentSpellIndex > 0;
            bool canGoRight = playerInventory.currentSpellIndex < playerInventory.collectedSpells.Count - 1;

            if (canGoLeft && canGoRight)
            {
                //Spells exist on both sides
                cyclePromptTextQ.text = "←Q";
                cyclePromptTextE.text = "E→";
            }
            else if (canGoLeft)
            {
                //At the end of the list
                cyclePromptTextQ.text = "←Q";
                cyclePromptTextE.text = "";
            }
            else if (canGoRight)
            {
                //At the front of the list. Can only cycle forwards.
                cyclePromptTextQ.text = "";
                cyclePromptTextE.text = "E→";
            }
        }
        //Else for if there are no spells or just 1 spell
        else
        {
            //No cycle prompt
            cyclePromptTextQ.text = "";
            cyclePromptTextE.text = "";
        }
    }
}