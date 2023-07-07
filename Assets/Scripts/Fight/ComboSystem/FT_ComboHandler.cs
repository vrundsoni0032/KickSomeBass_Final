using System.Linq;
using Core.EventSystem;
using Fight.Events;
using Fight.Fighter;

namespace Fight.ComboSystem
{   
public class FT_ComboHandler
{
    private FT_ComboActionSequence[] m_comboSequences;
    private FT_Fighter m_fighter;

    public FT_ComboHandler(FT_ComboActionSequence[] aCoboActionSequences, FT_Fighter aFighter)
    {
        m_comboSequences = aCoboActionSequences;
        m_fighter = aFighter;
        GameUtil.EventManager.RegisterSubscriber(HandleAbilitySuccessEvent, FT_AbilitySucessEvent.EventID);
    }

    public void Update()
    {
        if (m_comboSequences != null)
        {
            foreach (FT_ComboActionSequence combo in m_comboSequences)
            {
                combo.Update();
            }
        }
    }

    void OnNewAbilityPerformed(string aAbilityName)
    {
        string successfulComboAbility="";
        foreach (FT_ComboActionSequence combo in m_comboSequences)
        {
            successfulComboAbility = combo.CheckForCombo(aAbilityName);

            if (successfulComboAbility != null) break;
        }
            //If successfulComboAbility was assigned ,a combo was successful ,so add ability event to trigger combo ability
        if (successfulComboAbility != null)
        {
            //if (m_fighter.GetID() == FightUtil.GetAI().GetID())
            //{
            //    YCLogger.Info("ComboHandler", "Performed combo action - " + successfulComboAbility);
            //}

            if (m_fighter.GetID() != FightUtil.GetAI().GetID()) // temp not using for AI right now.
            {
                GameUtil.EventManager.AddEvent(new FT_AbilityEvent(m_fighter, successfulComboAbility, m_fighter.GetTransform().forward));
            }
        }
    }

    void HandleAbilitySuccessEvent(KSB_IEvent aEvent)
    {
        FT_AbilitySucessEvent abilitySuccessEvent= (FT_AbilitySucessEvent)aEvent;
        if(abilitySuccessEvent.Fighter.GetID() == m_fighter.GetID())
        {
            OnNewAbilityPerformed(abilitySuccessEvent.AbilityName);
        }
    }

    public string[] GetComboSequence(string comboName)
    {
        FT_ComboActionSequence comboSequence = m_comboSequences.FirstOrDefault(combo => combo.ComboActionName == comboName);
        YCLogger.Assert("ComboHandler", comboSequence != null, "Combo sequence is null. " + comboName + " not found.");

        return comboSequence.ActionSequence;
    }

    public void UnRegisterToEventSystem() =>
        GameUtil.EventManager.UnRegisterSubscribers(HandleAbilitySuccessEvent);
}
}