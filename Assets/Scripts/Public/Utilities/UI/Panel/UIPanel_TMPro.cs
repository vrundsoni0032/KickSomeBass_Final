using UnityEngine;
using UnityEngine.UI;

public class UIPanel_TMPro : UIPanel
{
    [SerializeField] private Color32 m_IdleColor;
    [SerializeField] private Color32 m_HoverColor;
    [SerializeField] private Color32 m_SelectedColor;

    [SerializeField] private float m_IdleSize;
    [SerializeField] private float m_HoverSize;
    [SerializeField] private float m_SelectedSize;

    protected override void OnAwake(Graphic buttonTargetGraphic)
    {
        if (buttonTargetGraphic as TMPro.TextMeshProUGUI)
        {
            ((TMPro.TextMeshProUGUI)buttonTargetGraphic).faceColor = m_IdleColor;
            ((TMPro.TextMeshProUGUI)buttonTargetGraphic).fontSize = m_IdleSize;
        }
    }

    protected override void Internal_OnButtonEnter(Graphic buttonTargetGraphic)
    {
        if (buttonTargetGraphic as TMPro.TextMeshProUGUI)
        {
            ((TMPro.TextMeshProUGUI)buttonTargetGraphic).faceColor = m_HoverColor;
            ((TMPro.TextMeshProUGUI)buttonTargetGraphic).fontSize = m_HoverSize;
        }
    }

    protected override void Internal_OnButtonClick(Graphic buttonTargetGraphic)
    {
        if (buttonTargetGraphic as TMPro.TextMeshProUGUI)
        {
            ((TMPro.TextMeshProUGUI)buttonTargetGraphic).faceColor = m_SelectedColor;
            ((TMPro.TextMeshProUGUI)buttonTargetGraphic).fontSize = m_SelectedSize;
        }
    }
    protected override void Internal_OnButtonExit(Graphic buttonTargetGraphic)
    {
        if (buttonTargetGraphic as TMPro.TextMeshProUGUI)
        {
            ((TMPro.TextMeshProUGUI)buttonTargetGraphic).faceColor = m_IdleColor;
            ((TMPro.TextMeshProUGUI)buttonTargetGraphic).fontSize = m_IdleSize;
        }
    }
}