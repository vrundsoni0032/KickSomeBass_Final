using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIPanelButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private Graphic m_TargetGraphic; 

    [SerializeField] private UnityEvent m_OnSelectEvents;
    [SerializeField] private UnityEvent m_OnDeselectEvents;

    private UIPanel m_parentPanel;

    public void OnPointerEnter(PointerEventData eventData) { m_parentPanel.HandlePointerEnter(this); }
    public void OnPointerClick(PointerEventData eventData) { m_parentPanel.OnButtonClick(); }
    public void OnPointerExit(PointerEventData eventData) { m_parentPanel.OnButtonExit(); }
    public void InvokeOnSelectEvents() { if(m_OnSelectEvents != null) { m_OnSelectEvents.Invoke(); } }
    public void InvokeOnDeselectEvents() { if (m_OnDeselectEvents != null) { m_OnDeselectEvents.Invoke(); } }

    public void SetParentPanel(UIPanel parentPanel) { m_parentPanel = parentPanel; }
    public Graphic GetTargetGraphic() { return m_TargetGraphic; }
}
