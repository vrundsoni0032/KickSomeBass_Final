using InteractiveMap.DialogueSystem;
using UnityEngine;

namespace InteractiveMap.NPC
{
    public class IM_MamaDuck : MonoBehaviour
    {
        public void SetAppropriateDialogueSet()
        {
            LinearDialogueDriver dialogueDriver = GetComponent<LinearDialogueDriver>();

            if (GameUtil.PlayerState.GetPlayerStoryEvent(PlayerStoryEvent.WonFirstFight) && dialogueDriver.GetCurrentDialogueSet() < 1)
            {
                dialogueDriver.SetNewDialogueSet(1);
            }
        }
    }
}
