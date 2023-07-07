using UnityEngine;

namespace Fight.ComboSystem
{
[CreateAssetMenu(fileName = "newComboList", menuName = "KSBUtilities/Data/ComboList")]
public class SO_ComboList : ScriptableObject
{
    [SerializeField]FT_ComboActionSequence[] m_ComboActionSequences;

    public FT_ComboActionSequence[] GetComboActionSequence() => m_ComboActionSequences;
}
}