using UnityEngine;

namespace Fight.ComboSystem
{
[System.Serializable]
public class FT_ComboActionSequence
{
    [SerializeField] string[] m_actionSequence;
    [SerializeField] string m_comboAction;

    int m_nextCheckIndex = 0;

    float m_resetTime=1f;
    float m_resetCountDown;

    public void Update()
    {
        //Only run reset timer when atleast 1st action was checked successful
        if(m_nextCheckIndex != 0)
        {
            if (m_resetCountDown > 0) { m_resetCountDown -= Time.deltaTime; }
            else { ResetCombo(); }
        }
    }

    public string CheckForCombo(string actionName)
    {
        //Debug.LogWarning(m_nextCheckIndex);
        if(m_actionSequence[m_nextCheckIndex].Equals(actionName))   //if new action is same as the next action to check
        {
            m_nextCheckIndex++;
            m_resetCountDown = m_resetTime;
        }
        else if(m_actionSequence[0].Equals(actionName)) //if new action is not the next action ,reset,and check if the first action is similar and start combo check again
        {
            m_nextCheckIndex=1;
            m_resetCountDown = m_resetTime;
        }
        else
        {
            ResetCombo();
        }

        //if the m_nextCheckIndex was the final check ,combo was successful
        if(m_nextCheckIndex>m_actionSequence.Length - 1)
        {
            ResetCombo();
            return m_comboAction;
        }

        return null;
    }

    void ResetCombo()
    {
        //Debug.LogWarning("Reset "+ m_nextCheckIndex);
        m_nextCheckIndex = 0;
        m_resetCountDown = m_resetTime;
    }

    public string ComboActionName => m_comboAction;
    public string[] ActionSequence => m_actionSequence;
}
}