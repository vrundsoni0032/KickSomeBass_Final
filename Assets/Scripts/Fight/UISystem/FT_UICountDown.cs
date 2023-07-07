using TMPro;
using Core.EventSystem;
using Fight.Events;
using UnityEngine;

public class FT_UICountDown : MonoBehaviour
{
    [SerializeField] private TMP_Text m_text;
    [SerializeField] private int m_countdown;
    private bool m_enabled;

    private float m_counter;

    private void Start()
    {
        GameUtil.EventManager.RegisterSubscriber(HandleRoundEndEvent, FT_RoundOverEvent.EventID);
        GameUtil.EventManager.RegisterSubscriber(UnRegisterToEventSystem, KSB_EndSceneEvent.EventID);
    }

    public void StartFight()
    {
        m_enabled = true;
        gameObject.SetActive(true);
        m_counter = m_countdown;
        m_text.text = m_countdown.ToString();
    }

    public void HandleRoundEndEvent(KSB_IEvent gameEvent) => StartFight();
    
    private void Update()
    {
        if (!m_enabled) { return; }

        m_counter -= Time.deltaTime;

        if (m_counter > - 1.0f)
        {
            if (m_counter > 0.0f) { m_text.text = ((int)m_counter).ToString(); }
            else { m_text.text = "GO!!"; }
        }
        else
        {
            m_enabled = false;
            gameObject.SetActive(false);
            GameUtil.EventManager.AddEvent(new FT_RoundBeginEvent());
        }
    }

    public void UnRegisterToEventSystem(KSB_IEvent gameEvent)
    {
        GameUtil.EventManager.UnRegisterSubscribers(HandleRoundEndEvent);
    }

}