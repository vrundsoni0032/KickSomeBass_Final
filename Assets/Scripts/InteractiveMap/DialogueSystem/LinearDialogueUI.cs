using TMPro;
using Core.EventSystem;
using Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace InteractiveMap.DialogueSystem
{
    public class LinearDialogueUI : MonoBehaviour
    {
        [SerializeField] private Canvas m_dialogueCanvas;

        [SerializeField] private TMP_Text m_nameText;
        [SerializeField] private GameObject m_nameObject;

        [SerializeField] private Image m_image;

        [SerializeField] private TMP_Text m_dialogueText;
        [SerializeField] private GameObject m_dialogueObject;

        [SerializeField] private TMP_Text m_leftText;
        [SerializeField] private GameObject m_leftObject;
        private UnityEvent m_leftAction;

        [SerializeField] private TMP_Text m_rightText;
        [SerializeField] private GameObject m_rightObject;
        private UnityEvent m_rightAction;

        public void Start()
        {
            GameUtil.EventManager.RegisterSubscriber(HandleChangeDialogueEvent, IM_ChangeDialogueEvent.EventID);
            GameUtil.EventManager.RegisterSubscriber(HandleToggleDialogueBoxEvent, IM_ToggleDialogueBoxEvent.EventID);
            GameUtil.EventManager.RegisterSubscriber(HandleOverDrawnCanvasEvent, KSB_EscapeEvent.EventID);
            GameUtil.EventManager.RegisterSubscriber(UnRegisterToEventSystem, KSB_EndSceneEvent.EventID);
        }

        public void HandleChangeDialogueEvent(KSB_IEvent gamEvent)
        {
            IM_ChangeDialogueEvent dialogueEvent = (IM_ChangeDialogueEvent)gamEvent;

            m_nameText.text = dialogueEvent.Name;
            m_image.sprite = dialogueEvent.Image;

            m_dialogueText.text = dialogueEvent.Dialogue.Statement;
            m_rightText.text = dialogueEvent.Dialogue.RightChoice;
            m_rightAction = dialogueEvent.Dialogue.RightAction;

            if (dialogueEvent.Dialogue.ShowLeftChoice)
            {
                m_leftObject.SetActive(true);
                m_leftText.text = dialogueEvent.Dialogue.LeftChoice;
                m_leftAction = dialogueEvent.Dialogue.LeftAction;
            }
            else
            {
                m_leftObject.SetActive(false);
            }
        }

        public void HandleToggleDialogueBoxEvent(KSB_IEvent gamEvent)
        {
            if (IM_Core.isAnyCanvasActive) { return; }

            m_dialogueCanvas.enabled = true;
            IM_Core.isAnyCanvasActive = true;

            GameUtil.EventManager.AddEvent(new KSB_MouseStateEvent(true));
            GameUtil.EventManager.AddEvent(new KSB_InterruptGameplayInputEvent(true));

        }

        public void HandleOverDrawnCanvasEvent(KSB_IEvent gameEvent)
        {
            if(m_dialogueCanvas.enabled == false) { return; }

            m_dialogueCanvas.enabled = false;
            IM_Core.isAnyCanvasActive = false;
            GameUtil.EventManager.AddEvent(new KSB_MouseStateEvent(false));
            GameUtil.EventManager.AddEvent(new KSB_InterruptGameplayInputEvent(false));
        }

        public void OnRightClick() => m_rightAction.Invoke();
        public void OnLeftClick() => m_leftAction.Invoke();

        private void UnRegisterToEventSystem(KSB_IEvent gameEvent) =>
            GameUtil.EventManager.UnRegisterSubscribers(HandleChangeDialogueEvent, HandleToggleDialogueBoxEvent, HandleOverDrawnCanvasEvent);
    }
}