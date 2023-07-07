using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Fight.UISystem
{
public class FT_FighterUI : MonoBehaviour
{
    [SerializeField] private GameObject m_HealthBar;
    [SerializeField] private GameObject m_StaminaBar;
    [SerializeField] private TMP_Text m_score;
    private int m_winCount = 0;

    public void UpdateHealthBar(float changeAmount, float maxAmount) =>
        m_HealthBar.GetComponent<Image>().fillAmount = m_HealthBar.GetComponent<Image>().fillAmount + ( changeAmount/ maxAmount);
    
    public void UpdateStaminaBar(float changeAmount, float maxAmount) =>
        m_StaminaBar.GetComponent<Image>().fillAmount = m_StaminaBar.GetComponent<Image>().fillAmount + ( changeAmount/ maxAmount);

    public void IncrementWinCount() => m_score.text = (++m_winCount).ToString();
    
    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
        m_HealthBar.SetActive(value);
        m_StaminaBar.SetActive(value);
    }
}
}