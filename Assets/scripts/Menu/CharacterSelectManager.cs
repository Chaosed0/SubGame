using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{
    public ButtonActionSuccess[] characterSelectSuccess;
    public Button startButton;
    public Image[] slotImageComponents;
    public Sprite[] slotProfileSprites;
    private int[] selectedCharacterIDs = new int[3];
    
    private int maxCharacterID = 5;
	
	void Start()
    {
        GameNotReady();
        for (int i = 0; i < selectedCharacterIDs.Length; i++)
        {
            selectedCharacterIDs[i] = -1;
            SetSlotImage(i, -1);
        }
    }

    public void AddRemoveCharacter(int characterID)
    {
        if (characterID >= 0 && characterID <= maxCharacterID
            && selectedCharacterIDs.Contains(characterID))
        {
            RemoveCharacter(characterID);
        }
        else
        {
            AddCharacter(characterID);
        }
    }

    public void RemoveSelectedCharacterFromSlot(int slot)
    {
        RemoveCharacter(selectedCharacterIDs[slot]);
    }

    private void RemoveCharacter(int characterID)
    {
        for (int slotIdx = 0; slotIdx < selectedCharacterIDs.Length; slotIdx++)
        {
            // if slot has the character
            if (selectedCharacterIDs[slotIdx] == characterID)
            {
                characterSelectSuccess[characterID].OnSuccessEvents.Invoke();
                selectedCharacterIDs[slotIdx] = -1;
                SetSlotImage(slotIdx, -1);
                GameNotReady();
                return;
            }
        }

    }

    private void AddCharacter(int characterID)
    {
        for (int slotIdx = 0; slotIdx < selectedCharacterIDs.Length; slotIdx++)
        {
            int selectedCharacterID = selectedCharacterIDs[slotIdx];
            // if slot doesn't have a character
            if (selectedCharacterID == -1)
            {
                characterSelectSuccess[characterID].OnSuccessEvents.Invoke();
                selectedCharacterIDs[slotIdx] = characterID;
                SetSlotImage(slotIdx, characterID);
                CheckIfAllCharactersSelected();
                return;
            }
        }
    }

    private void CheckIfAllCharactersSelected()
    {
        if (!selectedCharacterIDs.Contains(-1))
        {
            GameStartData.playingCharacterIDs = selectedCharacterIDs;
            startButton.interactable = true;
        }
    }

    private void GameNotReady()
    {
        startButton.interactable = false;
    }

    private void SetSlotImage(int slot, int characterID)
    {
        slotImageComponents[slot].sprite = slotProfileSprites[characterID + 1];
    }
}
