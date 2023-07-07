using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable] public class UIAxisValueMapping 
{
    public string AxisLiteralToNavigate; 
    [Range(0, 30000)]public float scrollStep;
    [HideInInspector]public float scrollStepCounter;
}

public abstract class UIPanel : MonoBehaviour
{
    [SerializeField] protected List<UIPanelButton> m_panelButtons;

    [SerializeField] private List<KeyCode> m_ToSelectKey;

    [SerializeField] private List<UIAxisValueMapping> m_AxisValueMappingToNavigate;

    private int m_selectedbuttonIndex = 0;
    private int m_previousSelectedButton = 0;

    private void Awake()
    {
        foreach(UIPanelButton panelButton in m_panelButtons) 
        {
            panelButton.SetParentPanel(this); 
            OnAwake(panelButton.GetTargetGraphic());
        }

        OnButtonEnter();
    }

    private void OnButtonEnter() { Internal_OnButtonEnter(m_panelButtons[m_selectedbuttonIndex].GetTargetGraphic()); }
    public void OnButtonClick()
    {
        m_panelButtons[m_previousSelectedButton].InvokeOnDeselectEvents();

        Internal_OnButtonClick(m_panelButtons[m_selectedbuttonIndex].GetTargetGraphic());

        m_previousSelectedButton = m_selectedbuttonIndex;
        m_panelButtons[m_selectedbuttonIndex].InvokeOnSelectEvents();
    }
    public void OnButtonExit() { Internal_OnButtonExit(m_panelButtons[m_selectedbuttonIndex].GetTargetGraphic()); }

    private void Update()
    {
        foreach(UIAxisValueMapping axisValueMapping in m_AxisValueMappingToNavigate)
        {
            axisValueMapping.scrollStepCounter += Input.GetAxisRaw(axisValueMapping.AxisLiteralToNavigate);

            if (axisValueMapping.scrollStepCounter >= axisValueMapping.scrollStep)  { ChangeButtonIndex(-1); axisValueMapping.scrollStepCounter = 0.0f; }
            if (axisValueMapping.scrollStepCounter <= -axisValueMapping.scrollStep) { ChangeButtonIndex(1); axisValueMapping.scrollStepCounter = 0.0f; }
        }

        foreach (KeyCode keyCode in m_ToSelectKey)
        {
            if (Input.GetKeyDown(keyCode)) { OnButtonClick(); }
        }
    }

    public void HandlePointerEnter(UIPanelButton panelButton) { ChangeButtonIndex(m_panelButtons.IndexOf(panelButton) - m_selectedbuttonIndex); }

    private void ChangeButtonIndex(int changeValue)
    {
        OnButtonExit();

        if(m_selectedbuttonIndex + changeValue >= m_panelButtons.Count) { m_selectedbuttonIndex = 0; }
        else if (m_selectedbuttonIndex + changeValue < 0) { m_selectedbuttonIndex = m_panelButtons.Count - 1; }
        else { m_selectedbuttonIndex += changeValue; }

        OnButtonEnter();
    }

    protected abstract void OnAwake(Graphic buttonTargetGraphic);
    protected abstract void Internal_OnButtonEnter(Graphic buttonTargetGraphic);
    protected abstract void Internal_OnButtonClick(Graphic buttonTargetGraphic);
    protected abstract void Internal_OnButtonExit(Graphic buttonTargetGraphic);
}