using UnityEngine;
using UnityEngine.Events;

namespace InteractiveMap.DialogueSystem
{
    [System.Serializable] public class Dialogue
    {
        [TextArea(3, 10), SerializeField] public string m_statement;

        [SerializeField] private string m_rightChoice;
        [SerializeField] private UnityEvent m_RightAction;


        [SerializeField] private bool m_showLeftChoice;
        [SerializeField] private string m_leftChoice;
        [SerializeField] private UnityEvent m_LeftAction;

        public string Statement => m_statement;

        public string RightChoice => m_rightChoice;
        public UnityEvent RightAction => m_RightAction;


        public bool ShowLeftChoice => m_showLeftChoice;
        public string LeftChoice => m_leftChoice;
        public UnityEvent LeftAction => m_LeftAction;
    }
}