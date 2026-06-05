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
    public TextMeshProUGUI cyclePromptText;


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
        //Find the player inventory component in scene execution
        playerInventory = Object.FindFirstObjectByType<PlayerSpellInventory>();

        if (playerInventory != null)
        {
            //Connecct UpdateUI method to the inventory event notification list
            playerInventory.OnSpellChanged += UpdateUI;
        }

        //Execute initially to style the card as empty startup
        UpdateUI();
    }

    void OnDestroy()
    {
        //cleanup event links when scenes change to prevent errors
        if (playerInventory != null)
        {
            playerInventory.OnSpellChanged -= UpdateUI;
        }
    }

    private void UpdateUI()
    {
        if (cardDisplayImage == null) return;

        //Safety check to deafult to empty if player script is missing
        //First set to none
        SpellType activeSpell = SpellType.None; 
        //And if there is a spell, change it to the equipped spell
        if (playerInventory != null)
        {
            activeSpell = playerInventory.EquippedSpell;
        }

        //Switch color properties of card UI based on active Enum flag
        switch (activeSpell)
        {
            case SpellType.RedPush:
                cardDisplayImage.color = redPushColor;
                cardNameText.text = "PUSH";
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
        //Conditional for adding Q+E control hint
        if (playerInventory != null && playerInventory.collectedSpells.Count > 1)
        {
            cyclePromptText.text = "<-Q                 E->";
        }
        else
        {
            cyclePromptText.text = "";
        }
    }
}