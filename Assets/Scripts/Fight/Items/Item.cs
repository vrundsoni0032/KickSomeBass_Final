using UnityEngine;

namespace Fight
{

public class Item : ScriptableObject
{
    [SerializeField] private string m_name;

    public string GetName() { return m_name; }
    public virtual void Consume(PlayerState fighterState) { }
}
}
