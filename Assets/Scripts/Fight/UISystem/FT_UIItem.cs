using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FT_UIItem : MonoBehaviour
{
    [SerializeField] Image m_Image;
    [SerializeField] Text m_TextName;
    [SerializeField] Text m_TextQuantity;
   
    public void SetItem(Sprite aItemSprite,string aItemName,int aQuantity)
    {
        if (m_Image != null)
        {
            m_Image.sprite = aItemSprite;
            m_Image.color= Color.white;
        }
        if(m_TextName != null)
        m_TextName.text = aItemName;

        if(m_TextQuantity != null)
        m_TextQuantity.text = aQuantity.ToString();
    }

    public void SetQuantity(int aQuantity)
    {
        m_TextQuantity.text = aQuantity.ToString();
    }

    public void ClearItem()
    {
        m_TextQuantity.text = "";
        m_Image.color = new Color(m_Image.color.r, m_Image.color.g, m_Image.color.b,0); //Disable color
    }
}
