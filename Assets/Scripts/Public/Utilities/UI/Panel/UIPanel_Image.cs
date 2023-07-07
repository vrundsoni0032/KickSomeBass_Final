using UnityEngine;
using UnityEngine.UI;

public class UIPanel_Image : UIPanel
{
    [SerializeField] private Sprite m_IdleSprite;
    [SerializeField] private Sprite m_HoverSprite;
    [SerializeField] private Sprite m_SelectedSprite;

    protected override void OnAwake(Graphic buttonTargetGraphic)
    {
        if (buttonTargetGraphic as Image)
        {
            ((Image)buttonTargetGraphic).sprite = m_IdleSprite;
        }
    }

    protected override void Internal_OnButtonEnter(Graphic buttonTargetGraphic)
    {
        if (buttonTargetGraphic as Image)
        {
            ((Image)buttonTargetGraphic).sprite = m_HoverSprite;
        }
    }

    protected override void Internal_OnButtonClick(Graphic buttonTargetGraphic)
    {
        if (buttonTargetGraphic as Image)
        {
            ((Image)buttonTargetGraphic).sprite = m_SelectedSprite;
        }
    }
    protected override void Internal_OnButtonExit(Graphic buttonTargetGraphic)
    {
        if (buttonTargetGraphic as Image)
        {
            ((Image)buttonTargetGraphic).sprite = m_IdleSprite;
        }
    }
}