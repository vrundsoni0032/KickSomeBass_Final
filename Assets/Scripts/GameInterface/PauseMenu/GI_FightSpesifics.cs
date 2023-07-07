using UnityEngine;
using UnityEngine.UI;
using Fight.Player;

namespace GameInterface.PauseMenu 
{
public class GI_FightSpesifics : MonoBehaviour
{
    private FT_Player m_Player;
 
    public void Awake()
    {
    }
    public void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<FT_Player>();
    }

}
}
