using InteractiveMap.DialogueSystem;
using InteractiveMap.Events;
using UnityEngine;

namespace InteractiveMap.NPC
{
public class IM_MartyTheShopKeeper : MonoBehaviour
{
    public void EnterShop()
    {
        GameUtil.EventManager.AddEvent(new IM_ToggleShopEvent());
    }

    public void SetAppropriateDialogueSet()
    {
        LinearDialogueDriver dialogueDriver = GetComponent<LinearDialogueDriver>();

        if (GameUtil.PlayerState.GetPlayerStoryEvent(PlayerStoryEvent.WonFirstFight) && dialogueDriver.GetCurrentDialogueSet() < 1)
        {
            dialogueDriver.SetNewDialogueSet(1);
        }
        else if (GameUtil.PlayerState.GetPlayerStoryEvent(PlayerStoryEvent.CompletedMartyQuest))
        {
            dialogueDriver.SetNewDialogueSet(2);
        }
    }
}
}