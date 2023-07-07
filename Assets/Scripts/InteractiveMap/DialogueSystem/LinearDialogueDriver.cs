using System.Collections.Generic;
using Core.EventSystem;
using UnityEngine;
using UnityEngine.Events;

namespace InteractiveMap.DialogueSystem
{
    [System.Serializable] public struct DialogueSet
    {
        public bool bRandomizeDialogues;
        public Dialogue[] Dialogues;
    }

    public class LinearDialogueDriver : MonoBehaviour, IM_Interactable
    {
        [SerializeField] private string m_name;
        [SerializeField] private Sprite m_Image;

        [SerializeField] private bool m_randomizeSet;
        [SerializeField] private List<DialogueSet> m_dialogueSets;

        [SerializeField] private UnityEvent m_onTriggerAction;

        private int m_currentSet = 0;
        private int m_currentIndex = 0;

        //Start the dialogue
        public void TriggerInteraction()
        {
            //if there is an action to trigger on entering the dialogue, trigger it
            if (m_onTriggerAction != null)
            {
                m_onTriggerAction.Invoke();
            }
            //if you want to randomize the dialogue set, generate a random number and and go to that dialogue set
            if (m_randomizeSet)
            {
                m_currentSet = Random.Range(0, m_dialogueSets.Count);
            }
            
            GameUtil.EventManager.AddEvent(new IM_ToggleDialogueBoxEvent());
            //if the current dialogue set is meant to be randomized, pick a random start point
            if (m_dialogueSets[m_currentSet].bRandomizeDialogues)
            {
                m_currentIndex = Random.Range(0, m_dialogueSets[m_currentSet].Dialogues.Length);
            }
            GameUtil.EventManager.AddEvent(new IM_ChangeDialogueEvent(m_name, m_Image, m_dialogueSets[m_currentSet].Dialogues[m_currentIndex]));
        }

        // Change to the next dialogue option in the set
        public void ShowNextDialogue()
        {
            m_currentIndex++;

            if (m_currentIndex < m_dialogueSets[m_currentSet].Dialogues.Length)
            {
                GameUtil.EventManager.AddEvent(new IM_ChangeDialogueEvent(m_name, m_Image, m_dialogueSets[m_currentSet].Dialogues[m_currentIndex]));
            }
            else
            {
                m_currentIndex = 0;
                GameUtil.EventManager.AddEvent(new IM_ToggleDialogueBoxEvent());
                GameUtil.EventManager.AddEvent(new KSB_MouseStateEvent(false));
                GameUtil.EventManager.AddEvent(new KSB_InterruptGameplayInputEvent(false));
            }
        }

        //exit out of the dialogue
        public void EndConversation()
        {
            m_currentIndex = 0;
            GameUtil.EventManager.AddEvent(new KSB_EscapeEvent());
            GameUtil.EventManager.AddEvent(new KSB_MouseStateEvent(false));
            GameUtil.EventManager.AddEvent(new KSB_InterruptGameplayInputEvent(false));
        }

        //add fishbucks to the player
        public void RewardCoins(int bucks)
        {
            GameUtil.PlayerState.FishBucks += bucks;
        }
        //add xp to the player
        public void RewardXP(int xp)
        {
            GameUtil.PlayerState.ExperiencePoints += xp;
        }

        public void SetNewDialogueSet(int setIndex) => m_currentSet = setIndex;
        public int GetCurrentDialogueSet() => m_currentSet;
        //allows the player pass the tutorial bridge

        public void SetPlayerStoryEvent(int storyEvent) =>
            GameUtil.PlayerState.SetPlayerStoryEvent((PlayerStoryEvent)storyEvent, true);

        public void EnableGameObject(GameObject gameObjectToEnable) => gameObjectToEnable.SetActive(true);
        public void DisableGameObject(GameObject gameObjectToDisable) => gameObjectToDisable.SetActive(false);
        public void DestroyGameObject(GameObject gameObjectToDestroy) => Destroy(gameObjectToDestroy);
    }
}